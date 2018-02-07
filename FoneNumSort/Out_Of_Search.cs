using System;

namespace FoneNumSort
{
    class Out_Of_Search
    {
        public Boolean Found;
        public uint Steps;
        public Boolean Complete;

        public void Check()
        {
            if (Complete)
            {
                Steps = 0;
                Complete = false;
            }
        }

        public override string ToString()
        {
            string str;
            str = Found + " " + Steps;
            return str;
        }
    }
}
