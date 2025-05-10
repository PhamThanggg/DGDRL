namespace DGDiemRenLuyen.Common
{
    public class RoleConstants
    {
        public const string ADMIN = "ADMIN";
        public const string HDT = "HDT";
        public const string TK = "TK";
        public const string CBL = "CBL";
        public const string GV = "GV";
        public const string SV = "SV";


        public const string AdminOrHdt = ADMIN + "," + HDT;
        public const string SvOrCbl = SV + "," + CBL;
        public const string HdtOrTkOrCvhtOrCbl = HDT + "," + TK + "," + GV + "," + CBL;
    }
}
