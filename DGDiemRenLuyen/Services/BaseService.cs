using DGDiemRenLuyen.DTOs.Responses;

namespace DGDiemRenLuyen.Services
{
    public abstract class BaseService<T1, T2>
    {
        public T1 _dataRequest;
        public T2 _dataResponse;
        public T2 _exceptionDataResponse;
        public ApiResponse<T2> _responseResult;

        protected BaseService(
            string successMessageDefault = "")
        {
            _responseResult = new ApiResponse<T2>()
            {
                StatusCode = StatusCodes.Status200OK.ToString(),
                Messages = successMessageDefault
            };
        }

        public abstract void P1GenerateObjects();

        /// Check validate
        public abstract void P2PostValidation();

        /// Access DB 
        public abstract void P3AccessDatabase();

        /// data response 
        public abstract void P4GenerateResponseData();

        public ApiResponse<T2> Process(T1 dataRequest = default(T1))
        {
            try
            {
                _dataRequest = dataRequest;
                P1GenerateObjects();
                P2PostValidation();
                P3AccessDatabase();
                P4GenerateResponseData();
                _responseResult.Data = _dataResponse;
                return _responseResult;
            }
            catch (BaseException ex)
            {
                return new ApiResponse<T2>
                {
                    StatusCode = ex.StatusCode,
                    Messages = ex.Messages,
                    MessagesDetails = ex.MessagesDetails,
                    Data = _exceptionDataResponse,
                };
            }
            catch (Exception ex)
            {
                // Exception Log
                return new ApiResponse<T2>
                {
                    StatusCode = StatusCodes.Status500InternalServerError.ToString(),
                    Messages = "Lôi xử lý dữ liệu, vui lòng thử lại"
                };
            }
        }
    }
}
