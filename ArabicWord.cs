using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arabic.Mappers;
using R = Arabic.Resources;
namespace Arabic
{
    #region Extention Functuality
    public static class ArabicWordExtention
    {
        /// <summary>
        /// Remove N ArabicChars from ArabicWord and return new ArabicWord
        /// </summary>
        /// <param name="aw">The ArabicWord</param>
        /// <param name="Count">The Number Of ArabicChar you want to remove</param>
        /// <param name="startindex">start Index</param>
        /// <returns></returns>
        public static ArabicWord Remove(this ArabicWord aw, int Count, int startindex = 0)
        {
            try
            {
                var tmp = aw.Word.ToList();
                for (int i = startindex; i < Count; i++)
                {
                    if (tmp[0].HasShadeh)
                    {
                        var t = new ArabicChar(tmp[0].Char, tmp[0].Diac, false, tmp[0].ParentWord);
                        tmp.Insert(1, t);
                    }
                    tmp.RemoveAt(0);
                }
                return new ArabicWord(tmp);
            }
            catch (ArgumentOutOfRangeException)
            {
                return new ArabicWord();
            }
        }
        /// <summary>
        /// Convert List of ArabicChar to String
        /// </summary>
        /// <param name="ac"></param>
        /// <returns></returns>
        public static string ArCharArrayToString(this ArabicChar[] ac)
        {
            var y = string.Join("", ac.Where(x => x != null).Select(x => x.Char + (x.Diac != null ? x.Diac.Diac : "")));
            return y;

        }
    }
    #endregion
    /// <summary>
    /// Since Computes doesn't really deal with Arabic Characters i needed to re-invent the weel by interduce you to Arabic Project
    /// Each Arabic Word is a list of ArabicChars
    /// </summary>
    public class ArabicWord
    {
        #region Private Properties
        private IList<ArabicChar> _word;
        #endregion
        #region Public Properties
        public IList<ArabicChar> Word { get { return _word ?? new List<ArabicChar>(); } set { this._word = value; } }

        #endregion
        #region Getters
        /// <summary>
        /// If Someone nerdy ask to get the word backward only as ArabicWord
        /// </summary>
        public ArabicWord GetReversedArabicWord
        {
            get
            {
                var tmp = Word.Reverse().ToList();
                return new ArabicWord(tmp);
            }
        }
        /// <summary>
        /// If Someone nerdy ask to get the word backward only as string
        /// </summary>
        public string GetReversedArabicString
        {
            get
            {
                var tmp = Word.Reverse().ToList();
                return new ArabicWord(tmp).GetArabicString;
            }
        }
        /// <summary>
        /// return arabicChar from arabicWord at certain index
        /// </summary>
        /// <param name="Index">The Index</param>
        /// <returns></returns>
        public ArabicChar GetArabicCharAtIndex(int Index)
        {
            return Word.ElementAt(Index);
        }
        /// <summary>
        /// Get ArabicString with replaing the Shadeh char
        /// </summary>
        public string GetArabicStringWithoutShadeh
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var ac in Word)
                {
                    sb.Append(ac.GetArabicCharStringWithoutShadeh);
                }
                return sb.ToString();
            }
        }
        /// <summary>
        /// Convert ArabicWord to String
        /// </summary>
        public string GetArabicString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var ac in Word)
                {
                    sb.Append(ac.GetArabicCharString);
                }
                return sb.ToString();
            }
        }
        /// <summary>
        /// Putting Default Sokon on the word when it make since
        /// </summary>
        public ArabicWord GetArabicWordWithSokonByDefault
        {
            //the char is not the first char in the word
            //example of output
            // {diac char diac} ==> diac sokon diac
            // {diac char}==>diac sokon
            // {char diac}==>sokon diac
            get
            {
                var tmp = Word.ToList();
                for (int i = 0; i < tmp.Count; i++)
                {
                    if (i == 0)
                        continue;
                    if (!tmp[i].HasDiac)
                    {
                        try
                        {
                            if (tmp[i - 1].HasDiacWithoutSokon || tmp[i + 1].HasDiacWithoutSokon)
                            {
                                tmp[i].Diac = new Diacritic(R.sokon);
                            }
                        }
                        catch { }
                        try
                        {
                            if (tmp[i - 1].HasDiacWithoutSokon)
                            {
                                tmp[i].Diac = new Diacritic(R.sokon);
                            }
                        }
                        catch { }
                        try
                        {
                            if (tmp[i + 1].HasDiacWithoutSokon)
                            {
                                tmp[i].Diac = new Diacritic(R.sokon);
                            }
                        }
                        catch { }
                    }
                }
                return new ArabicWord(tmp);
            }
        }
        #endregion

        #region Special Getters
        /// <summary>
        /// Replace ArabicWord Shadeh
        /// </summary>
        public ArabicWord ReplaceShadeh
        {
            get
            {
                return new ArabicWord(GetArabicStringWithoutShadeh);
            }
        }
        public static ArabicWord RemoveNonArabicRelated(string word)
        {
            var s = new string(word.Where(x => !ArabicChar.CheckIfArabicRelated(x.ToString())).ToArray());
            return FromStringToArabicWordMapper.FromString(s);
        }
        /// <summary>
        /// Filterd ArabicWord doesn't has Shadeh or Diacterct or anything it just normal arabic characters
        /// </summary>
        public ArabicWord Filterd
        {
            get
            {
                List<ArabicChar> tmp = new List<ArabicChar>();
                foreach (var ac in Word)
                {
                    tmp.Add(new ArabicChar(ac.Char.ToString(), new Diacritic(), false, this));
                }
                return new ArabicWord(tmp);
            }

        }
        /// <summary>
        /// take arabicWord and return arabicWord With Shadeh replaced
        /// </summary>
        public ArabicWord ReplaceShadehFromArabicWord
        {
            get
            {
                List<ArabicChar> tmp = new List<ArabicChar>();
                foreach (var ac in Word)
                {
                    tmp.Add(ac.GetArabicCharWithoutShadeh);
                }
                return new ArabicWord(tmp);
            }

        }
        public int Length => Word.Count;

        #endregion
        #region Indexer
        public ArabicChar this[int index]  //indexer
        {
            get
            {
                if (index >= 0 && index < Word.Count)
                {
                    return Word[index];
                }
                else
                {
                    throw new System.IndexOutOfRangeException();
                }
            }
            set
            {
                if (index >= 0 && index < Word.Count)
                {
                    Word[index] = value;
                }
                else
                {
                    throw new System.IndexOutOfRangeException();
                }
            }
        }

        #endregion
        #region Ctor

        public ArabicWord()
        {
        }
        public ArabicWord(string word)
        {
            Word = FromStringToArabicWordMapper.FromString(word).Word;
        }

        public ArabicWord(IList<ArabicChar> _Word)
        {
            Word = _Word;
        }

        #endregion
    }
}
