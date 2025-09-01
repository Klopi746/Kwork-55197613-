namespace YG
{
    public partial class SavesYG
    {
        public int currentIndexLvl = -1;
        public int coins = 50;
        public string hint = "";
        public float volumeSound;
        public bool nOTads;
        public bool NOTads { 
            get { return nOTads; }
            set { 
                nOTads = value;
                YG2.SaveProgress();
            } 
        }
    }
}