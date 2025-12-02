namespace OJT_RAG.ModelViews.Company
{
    public class CompanyCreateModel
    {
        public long? MajorID { get; set; }
        public string Name { get; set; }
        public string Tax_Code { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public string Contact_Email { get; set; }
        public string Phone { get; set; }
        public string Logo_URL { get; set; }
        public bool Is_Verified { get; set; }
    }
}