using System;
using System.Collections.Generic;
using System.Text;

namespace MiffTheFox.Text
{
    internal class CharacterReader
    {
        protected int _Position = 0;

        public string Text { get; }
        public int Length => Text.Length;

        public CharacterReader(string text)
        {
            if (text is null) throw new ArgumentNullException(nameof(text), "Text cannot be null.");
            Text = text;
        }

        public void Skip(int value)
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "value must not be less than zero.");
            _Position += value;
        }

        public void Reset()
        {
            _Position = 0;
        }

        public char? Read()
        {
            if (_Position >= Length)
            {
                return null;
            }
            else
            {
                return Text[_Position++];
            }
        }

        public bool TryRead(out char c)
        {
            if (_Position >= Length)
            {
                c = default;
                return false;
            }
            else
            {
                c = Text[_Position++];
                return true;
            }
        }

        public bool TryReadString(int count, out string str)
        {
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count), "count must be greater than zero.");

            if (_Position >= Length)
            {
                str = null;
                return false;
            }
            else
            {
                if (_Position + count > Length)
                {
                    // read remainder of chars but still return false
                    str = Text.Substring(_Position);
                    _Position = str.Length;
                    return false;
                }
                else
                {
                    str = Text.Substring(_Position, count);
                    _Position += count;
                    return true;
                }
            }
        }

        public bool TryReadTo(char end, out string str)
        {
            if (_Position >= Length)
            {
                str = null;
                return false;
            }
            else
            {
                int index = Text.IndexOf(end, _Position);
                if (index == -1)
                {
                    str = Text.Substring(_Position);
                    _Position = Length;
                    return false;
                }
                else
                {
                    str = Text.Substring(_Position, index - _Position);
                    _Position = index + 1;
                    return true;
                }
            }
        }

        public override string ToString() => Text;
    }
}
