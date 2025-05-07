namespace JobBit.DTOs
{
    public class ResultEmailVerification
    {
        public string OTPCode {  get; set; }

        public ResultEmailVerification(string oTPCode)
        {
            OTPCode = oTPCode;
        }
    }
}
