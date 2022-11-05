namespace mc
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write(">  ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    return;

                var lexer = new Lexer(line);
                while(true)
                {
                    var token = lexer.nextToken();
                    if (token.Kind == SyntaxKind.endOfFileToken)
                        break;
                    Console.WriteLine($"{token.Kind}: '{token.Text}'");
                    if(token.Value != null)
                        Console.Write($"{token.Value}");

                    Console.WriteLine();
                }
            }
        }
    }

    enum SyntaxKind
    {
        numberToken,
        whitespaceToken,
        plusToken,
        minusToken,
        starToken,
        slashToken,
        openParenthesisToken,
        closeParenthesisToken,
        badToken,
        endOfFileToken
    }

    class SyntaxToken
    {
        public SyntaxKind Kind { get; }
        public int Position { get; }
        public string Text { get; }

        public object Value { get; }

        public SyntaxToken(SyntaxKind kind, int position, string text, object? value)
        {
            Kind = kind;
            Position = position;
            Text = text;
            Value = value;
        }



    }
    class Lexer
    {
        private readonly string _text;
        private int _position;

        private void next()
        {
            _position++;
        }
        public Lexer(string text)
        {
            _text = text;
        }

        private char current
        {
            get
            {
                if (_position >= _text.Length)
                    return '\0';
                return _text[_position];
            }
        }
        public SyntaxToken nextToken()
        {
            // numbers
            // + - * / ( )
            // whitespace

            if (_position >= _text.Length)
                return new SyntaxToken(SyntaxKind.endOfFileToken, _position, "\0",null);
            if (char.IsDigit(current))
            {
                var start = _position;

                while (char.IsDigit(current))
                    next();

                var length = _position - start;
                var text = _text.Substring(start, length);
                int.TryParse(text, out var value);
                return new SyntaxToken(SyntaxKind.numberToken, start, text, value);
            }

            if (char.IsWhiteSpace(current))
            {
                var start = _position;

                while (char.IsWhiteSpace(current))
                    next();

                var length = _position - start;
                var text = _text.Substring(start, length);
                int.TryParse(text, out var value);
                return new SyntaxToken(SyntaxKind.whitespaceToken, start, text, value);
            }

            if (current == '+')
                return new SyntaxToken(SyntaxKind.plusToken, _position++, "+", null);
            else if (current == '-')
                return new SyntaxToken(SyntaxKind.minusToken, _position++, "-", null);
            else if (current == '*')
                return new SyntaxToken(SyntaxKind.starToken, _position++, "*", null);
            else if (current == '/')
                return new SyntaxToken(SyntaxKind.slashToken, _position++, "/", null);
            else if (current == '(')
                return new SyntaxToken(SyntaxKind.openParenthesisToken, _position++, "(", null);
            else if (current == ')')
                return new SyntaxToken(SyntaxKind.closeParenthesisToken, _position++, ")", null);

            return new SyntaxToken(SyntaxKind.badToken, _position++, _text.Substring(_position-1, 1), null);
        }
    };
}