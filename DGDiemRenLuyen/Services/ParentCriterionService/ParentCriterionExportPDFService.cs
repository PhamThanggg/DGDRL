using DGDiemRenLuyen.DTOs.requsets;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using System.IO;
using System.Text;

namespace DGDiemRenLuyen.Services.ParentCriterionService
{
    public class ParentCriterionExportPDFService : BaseService<ParentCriterionGetListDetailRequest, Task<FileStreamResult>>
    {
        private readonly IParentCriteriaRepository _parentCriteriaRepository;
        private List<ParentCriterionDto>? parentCriterions;

        public ParentCriterionExportPDFService(
            IParentCriteriaRepository parentCriteriaRepository,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _parentCriteriaRepository = parentCriteriaRepository;
        }

        public override void P1GenerateObjects()
        {
        }

        public override void P2PostValidation()
        {

        }

        public override void P3AccessDatabase()
        {
            var task = _parentCriteriaRepository
                .GetParentCriteriaByStudentIdAsync(_dataRequest.scoreId, _dataRequest.TimeId);

            task.Wait();
            parentCriterions = task.Result;

            if (parentCriterions == null)
            {
                throw new BaseException
                {
                    StatusCode = StatusCodes.Status404NotFound.ToString(),
                    Messages = "Form đánh giá này không tồn tại!"
                };
            }
        }

        public override void P4GenerateResponseData()
        {
            

        }

        public static string ToRoman(int number)
        {
            if (number < 1) return "";
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900);
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            return "";
        }
    }
}
