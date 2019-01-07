using System.Collections.Generic;
using R = Arabic.Resources;
namespace Arabic
{
    #region Diacritics Enum
    public enum Diacritics//الحركات
    {
        _none,
        fatha,
        kasra,
        ddameh,
        sokon,
        tnoen_kasra,
        tnoen_ddameh,
        tnoen_fatha
    }
    #endregion
    /// <summary>
    /// The ArabicChar may or may not has Diacritic on it 
    /// </summary>
    public class Diacritic
    {
        #region Private Propreites

        private static List<string> Diacritics = new List<string> { R._9ameh, R.kasra, R.fatha, R.sokon };
        private static List<string> Tanween = new List<string> { R.tnoen_9amma, R.tnoen_fatha, R.tnoen_kasra };
        private string _diac = "";
        private Diacritics EnumDiac;
        #endregion
        #region Public Properties
        public string Diac
        {
            get { return this._diac; }
            set
            {
                if (Diacritics.Contains(value.ToString()) || Tanween.Contains(value.ToString()))
                {
                    this._diac = value;
                    if (value.Equals(R.fatha))
                    {
                        EnumDiac = Arabic.Diacritics.fatha;
                    }
                    else if (value.Equals(R.kasra))
                    {
                        EnumDiac = Arabic.Diacritics.kasra;

                    }
                    else if (value.Equals(R.sokon))
                    {
                        EnumDiac = Arabic.Diacritics.sokon;

                    }
                    else if (value.Equals(R._9ameh))
                    {
                        EnumDiac = Arabic.Diacritics.ddameh;

                    }
                    else if (value.Equals(R.tnoen_9amma))
                    {
                        EnumDiac = Arabic.Diacritics.tnoen_ddameh;

                    }
                    else if (value.Equals(R.tnoen_fatha))
                    {
                        EnumDiac = Arabic.Diacritics.tnoen_fatha;

                    }
                    else if (value.Equals(R.tnoen_kasra))
                    {
                        EnumDiac = Arabic.Diacritics.tnoen_kasra;

                    }
                }
            }
        }

        #endregion
        #region Getters
        public Diacritics AsEnum => EnumDiac;

        #endregion
        #region Special Getters

        #endregion
        #region Checkers
        public bool IsTanween { get { return Tanween.Contains(Diac.ToString()); } }
        public bool IsSokon { get { return R.sokon == Diac; } }
        public bool IsDiac { get { return Diacritics.Contains(Diac.ToString()); } }
        #endregion

        #region Functions
        public static bool CheckIfDiac(string c)
        {
            return Diacritics.Contains(c.ToString()) || Tanween.Contains(c.ToString());
        }

        #endregion

        #region Ctor

        public Diacritic(string _Diac)
        {
            Diac = _Diac;
        }

        public Diacritic()
        {
        }
        #endregion

    }
}
