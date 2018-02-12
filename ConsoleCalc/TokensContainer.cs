using ConsoleCalc.TokenHierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalc {
    public class TokensContainer {
        List<Token> tokens;
        int i;

        /// <summary>
        /// Get an iterator for the tokens.
        /// </summary>
        public IEnumerable<Token> Tokens => tokens;
        /// <summary>
        /// Get the iterator's position.
        /// </summary>
        public int Position => i;

        public TokensContainer() {
            tokens = new List<Token>();
        }

        /// <summary>
        /// Add a token to the container.
        /// </summary>
        /// <param name="token">The token to add.</param>
        public void AddToken(Token token) {
            tokens.Add(token);
            Rewind();
        }

        /// <summary>
        /// Reset the iterator to the start.
        /// </summary>
        public void Rewind() {
            i = 0;
        }

        /// <summary>
        /// Return the next token and advance the iterator.
        /// </summary>
        /// <returns>The next token.</returns>
        public Token Forward() {
            return tokens[i++];
        }

        /// <summary>
        /// Peek the next token without advancing the iterator.
        /// </summary>
        /// <returns>The next token.</returns>
        public Token PeekForward() {
            return tokens[i];
        }

        /// <summary>
        /// Peek the next token that is offset from the current iterator's position.
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public Token PeekForward(int offset) {
            return tokens[i + offset];
        }

        /// <summary>
        /// Whether there is a next token to be read.
        /// </summary>
        /// <returns>True if there is, false if not</returns>
        public bool HasNext() {
            return i < tokens.Count;
        }

        /// <summary>
        /// Whether the next n tokens are available for reading.
        /// </summary>
        /// <param name="n">N tokens</param>
        /// <returns>True if there is, false if not.</returns>
        public bool HasNext(int n) {
            return i + (n - 1) < tokens.Count;
        }
    }
}
