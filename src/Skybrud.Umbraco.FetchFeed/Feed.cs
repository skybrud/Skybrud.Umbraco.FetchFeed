using System.Text;

namespace Skybrud.Umbraco {
    
    public class Feed {

        #region Properties

        public string Alias { get; set; }
        
        public string Url { get; set; }
        
        public string Path { get; set; }
        
        public double Interval { get; set; }

        public Encoding Encoding { get; set; }

        #endregion

        #region Constructors

        public Feed() {
            Encoding = Encoding.Default;
        }

        #endregion

    }

}