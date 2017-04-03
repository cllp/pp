using System;

namespace PP.Core.Model
{
    [Serializable]
    public class Varience : ModelBase
    {
        public string Type { get; set; }
        string _prop;
        object _newValue;
        object _oldValue;
        
        public string Prop
        {
            get { return _prop; }
            set { _prop = value; }
        }
        public object NewValue
        {
            get { return _newValue; }
            set { _newValue = value; }
        }

        public object OldValue
        {
            get { return _oldValue; }
            set { _oldValue = value; }
        }

    }
}
