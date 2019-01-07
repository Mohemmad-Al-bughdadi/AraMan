using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R = Arabic.Resources;
using Arabic.Mappers;
namespace Arabic
{
    /// <summary>
    /// Every Arabic Word is a list of ArabicChar 
    /// Arabic Char Is a Char and a Diactrict and a boolean if it has Shadeh or not
    /// </summary>
    public class ArabicChar
    {
        #region Public Propreties
        public string Char { get; set; }
        public Diacritic Diac { get; set; }
        public bool HasShadeh { get; set; } = false;
        //Check if this Arabic char is connected to the previous char
        public bool ConnectedToPrev
        {
            get
            {
                try
                {
                    var index = ParentWord.Word.IndexOf(this);
                    var prevChar = ParentWord != null && index != 0 ? ParentWord[index - 1] : null;
                    return (prevChar != null ? !NotConnected_ToNext.Contains(prevChar.Char.Last().ToString()) && !NotConnected_ToPrev.Contains(this.Char.Last().ToString()) : false);
                }
                catch (Exception)
                {

                    return false;
                }
            }
        }
        //Check if this Arabic char is connected to the next char
        public bool ConnectedToNext
        {

            get
            {
                try
                {
                    var index = ParentWord.Word.IndexOf(this);
                    var nextChar = ParentWord != null && index != ParentWord.Length - 1 ? ParentWord[index + 1] : null;
                    return (nextChar != null ? !NotConnected_ToPrev.Contains(nextChar.Char.First().ToString()) && !NotConnected_ToNext.Contains(this.Char.First().ToString()) : false);
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        #endregion
        #region Extra

        //Adding to ArabicChar to get a string
        public static string operator +(ArabicChar first, ArabicChar second)
        {
            return first.GetArabicCharString + (second != null ? second.GetArabicCharString : String.Empty);
        }
        //each ArabicChar is Derived from a word
        public ArabicWord ParentWord { get; set; }
        public int Length => Char.Length;
        #endregion
        #region private Lists
        //here i collect all the possible Arabic Char to make a list of
        //الأحرف الصوتية
        private static List<string> Vowles = new List<string> { R.alef, R.wow, R.yaa, R.alef_hamze_foq, R.alef_hamze_t7t };
        //الأحرف الشمسية
        private static List<string> Shamseeh = new List<string> { R.taa, R.thaa, R.dal, R.zal, R.raa, R.zay, R.sen, R.shen, R.sad, R.ddad, R._6aa, R.zhaa, R.lam, R.non };
        //الأحرف القمرية
        private static List<string> Qamareh = new List<string> { R.alef, R.alef_hamze_foq, R.alef_hamze_t7t, R.alef_mamdodeh, R.baa, R.gem, R._7aa, R.khaa, R.aien, R.ghien, R.faa, R.qaf, R.kaf, R.mem, R.haa, R.wow, R.yaa };
        //لا تنتمي للشمسية او القمرية او الصوتية
        private static List<string> Extra = new List<string> { R.alef_maqsora, R.taa_marbota, R.yaa_hamzeh, R.wow_hamzeh, R.hamzeh };
        private static List<string> Hamzat = new List<string> { R.alef, R.alef_hamze_foq, R.alef_hamze_t7t };
        //اب
        //بء
        //not linkable with any char when char is in thier left
        
        private static List<string> NotConnected_ToNext = new List<string> { R.alef, R.alef_hamze_foq, R.alef_hamze_t7t, R.alef_mamdodeh, R.raa, R.zay, R.dal, R.zal, R.hamzeh };
        //not linkable with char when char is in thier right
        private static List<string> NotConnected_ToPrev = new List<string> { R.hamzeh };

        #endregion
        //Checkers to this ArabicChar
        #region Checkers
        public bool HasDiac { get { return !string.IsNullOrEmpty(Diac.Diac) && Diac.IsDiac; } }
        public bool HasTanween { get { return !string.IsNullOrEmpty(Diac.Diac) && Diac.IsTanween; } }
        public bool HasDiacWithoutSokon { get { return HasDiac && !HasSokon; } }
        public bool HasSokon { get { return Diac.IsSokon; } }
        public bool HasSokonOrNone { get { return Diac.IsSokon || !HasDiac; } }
        public bool IsHamzeh { get { return Hamzat.Contains(Char.ToString()); } }
        public bool IsQamareh { get { return Qamareh.Contains(Char.ToString()); } }
        public bool IsShamseeh { get { return Shamseeh.Contains(Char.ToString()); } }
        public bool IsVowel { get { return Vowles.Contains(Char.ToString()); } }
        private bool IsExtra { get { return Extra.Contains(Char.ToString()); } }
        public bool IsArabic { get { return IsShamseeh || IsQamareh || IsExtra; } }

        //External Checkers
        public static bool CheckIfArabicChar(string c)
        {
            return Extra.Contains(c) || Qamareh.Contains(c) || Shamseeh.Contains(c);
        }
        public static bool CheckIfDiac(string c)
        {
            return Diacritic.CheckIfDiac(c);
        }
        public static bool CheckIfShadeh(string c)
        {
            return R.shadeh == c;
        }
        public static bool CheckIfArabicRelated(string c)
        {
            return CheckIfArabicChar(c) || CheckIfDiac(c) || CheckIfShadeh(c) || Diacritic.CheckIfDiac(c);
        }
        #endregion

        #region Ctor
        public ArabicChar(string _Char, Diacritic _Diac, bool _hasShadeh = false, ArabicWord _ParentWord = null) : this(_ParentWord)
        {
            Char = _Char;
            Diac = _Diac;
            HasShadeh = _hasShadeh;
        }
        public ArabicChar(string _Char, string _Diac, bool _hasShadeh = false, ArabicWord _ParentWord = null) : this(_ParentWord)
        {
            Char = _Char;
            Diac = FromDiacToDiactrectMapper.FromDiacChar(_Diac);
            HasShadeh = _hasShadeh;
        }
        public ArabicChar(string _Char, Diacritics _Diac, bool _hasShadeh = false, ArabicWord _ParentWord = null) : this(_ParentWord)
        {
            Char = _Char;
            Diac = FromDiacEnumToDiacMapper.FromDiacEnumToDiac(_Diac);
            HasShadeh = _hasShadeh;
        }
        public ArabicChar(ArabicWord _ParentWord = null)
        {
            Diac = new Diacritic();
            ParentWord = _ParentWord;
        }
        #endregion

        #region Getters
        //Convert ArabicChar to Arabic String with deffrent cases
        public string GetArabicCharString
        {
            get
            {
                return Char + (HasShadeh ? R.shadeh : String.Empty) + Diac.Diac;
            }
        }
        public string GetArabicCharStringWithoutShadeh
        {
            get
            {
                return HasShadeh ? Char + R.sokon + (HasShadeh ? (Char.ToString() + Diac.Diac) : string.Empty) : GetArabicCharString;
            }
        }
        public ArabicChar GetArabicCharWithoutShadeh
        {
            get
            {
                if (HasShadeh)
                {
                    return new ArabicChar(Char, R.sokon, false, ParentWord);
                }
                return new ArabicChar(Char, Diac, false, ParentWord);
            }
        }
        //Getters of the Lists
        public static List<string> getVowlesList => Vowles.ToList();
        public static List<string> getShamseehList => Shamseeh.ToList();
        public static List<string> getQamarehList => Qamareh.ToList();
        public static List<string> getExtraList => Extra.ToList();
        public static List<string> getHamzatList => Hamzat.ToList();
        public static List<string> getArabicList => Shamseeh.Concat(Qamareh).Concat(Extra).ToList();
        #endregion
    }
}
