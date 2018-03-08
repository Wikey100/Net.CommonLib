namespace DrawTools.Model
{
    public class FilterItem
    {
        public FilterItem(string code, string description)
        {
            this.code = code;
            this.description = description;
        }

        private string code;

        public string Code
        {
            get
            {
                return code;
            }
        }

        private string description;

        public string Description
        {
            get
            {
                return description;
            }
        }

        public override string ToString()
        {
            return this.code;
        }
    }
}