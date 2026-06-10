using CertificateSolver.Core.Interfaces;
using CertificateSolver.Infrastructure.Storage;
using CertificateSolver.Models;

namespace CertificateSolver.Core.Services
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _repository;
        private readonly IRequestStatusTransition _statusTransition;
        private readonly IIdempotencyKeyGenerator _keyGenerator;
        private readonly IIdempotencyStore _idempotencyStore;

        public RequestService(
            IRequestRepository repository,
            IRequestStatusTransition statusTransition,
            IIdempotencyKeyGenerator keyGenerator,
            IIdempotencyStore idempotencyStore)
        {
            _repository = repository;
            _statusTransition = statusTransition;
            _keyGenerator = keyGenerator;
            _idempotencyStore = idempotencyStore;
        }        

        public async Task<Guid> SubmitRequestAsync(string employeeId, CertificateType type, int copies, string reason, string role)
        {
            if(role != "employee")
                throw new UnauthorizedAccessException("Только сотрудники могут создавать заявки");

            var key = _keyGenerator.GenerateKey(employeeId, type.ToString(), copies, reason);
            if(_idempotencyStore.IsDublicate(key))
                throw new InvalidOperationException("Такой запрос уже был отправлен. Дубликат отклонён.");

            var request = new CertificateRequest
            {
                Id = Guid.NewGuid(),
                EmployeeId = employeeId,
                Type = type,
                Copies = copies,
                Reason = reason,
                Status = RequestStatus.Draft,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(request);

            

            if(!_statusTransition.TryTransition(request, RequestStatus.Submitted, role, out var error
              ))
                throw new InvalidOperationException(error);

            await _repository.UpdateAsync(request);

            _idempotencyStore.MarkProcessed(key);

            return request.Id;
        }
        public async Task<IEnumerable<CertificateRequest>> GetEmploypeeRequestsAsync(string employeeId)
        {
            return await _repository.GetByEmployeeAsync(employeeId);
        }



        public async Task<CertificateRequest> UpdateStatusAsync(Guid requestId, RequestStatus newStatus, string role, string rejectionReason = null)
        {
            var request = await _repository.GetByIdAsync(requestId);
            if (request == null)
                throw new ArgumentException("Заявка не найдена");

            if(!_statusTransition.TryTransition(request, newStatus, "accountant", out var error))
                throw new InvalidOperationException(error);

            if (newStatus == RequestStatus.Rejected)
            {
                if (string.IsNullOrWhiteSpace(rejectionReason))
                {
                    throw new InvalidOperationException(
                        "При отклонении заявки необходимо указать причину отказа"
                    );
                }
                request.RejectionReason = rejectionReason;
            }

            await _repository.UpdateAsync(request);
            return request;
        }
    }
}
