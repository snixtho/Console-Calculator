using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalc {
    public class TokenStringReader {
        string s;
        int i;

        /// <summary>
        /// Get the actual string the reader is reading from.
        /// </summary>
        public string Str => s;
        /// <summary>
        /// Get the position of the iterator.
        /// </summary>
        public int Position => i;

        public TokenStringReader(string s) {
            this.s = s;
            Rewind();
        }

        /// <summary>
        /// Reset the iterator to the start.
        /// </summary>
        public void Rewind() {
            i = 0;
        }

        /// <summary>
        /// Return the next character and advance the iterator.
        /// </summary>
        /// <returns>The next character.</returns>
        public int Forward() {
            return (int)s[i++];
        }

        /// <summary>
        /// Peek the next character without advancing the iterator.
        /// </summary>
        /// <returns>The next character.</returns>
        public int PeekForward() {
            return (int)s[i];
        }

        /// <summary>
        /// Peek the next character that is offset from the current iterator's position.
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public char PeekForward(int offset) {
            return (char)s[i + offset];
        }

        /// <summary>
        /// Whether there is a next character to be read.
        /// </summary>
        /// <returns>True if there is, false if not</returns>
        public bool HasNext() {
            return i < s.Length;
        }

        /// <summary>
        /// Whether the next n characters are available for reading.
        /// </summary>
        /// <param name="n">N characters-</param>
        /// <returns>True if there is, false if not.</returns>
        public bool HasNext(int n) {
            return i + (n - 1) < s.Length;
        }
    }
}
