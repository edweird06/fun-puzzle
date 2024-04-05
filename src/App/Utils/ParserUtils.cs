namespace App.Utils
{
    public class ParserUtils
    {
        /// <summary>
        /// Parse the input string and return a dictionary of dictionaries back to the caller
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Dictionary<string, object?> ParseString(string input)
        {
            // using object as the value in the Dictionary since the value could 
            // be an unknown amount of dictionaries
            var result = new Dictionary<string, object?>();
            var stack = new Stack<Dictionary<string, object?>>();
            stack.Push(result);

            var currentKey = string.Empty;
            foreach (var ch in input)
            {
                switch (ch)
                {
                    case '(':
                        if (!string.IsNullOrWhiteSpace(currentKey))
                        {
                            // this won't get entered until it hits the first 
                            // nested value, at which time "currentKey" will have a value
                            var newDict = new Dictionary<string, object?>();
                            stack.Peek().Add(currentKey.Trim(), newDict);
                            stack.Push(newDict);
                        }

                        // clear currentKey because it has either already been added to
                        // a dictionary or is invalid as we should be starting a fresh key
                        currentKey = string.Empty;
                        break;
                    case ')':
                        if (!string.IsNullOrWhiteSpace(currentKey))
                        {
                            // if currentKey has a value we need to enter it
                            // prior to closing the current collection
                            stack.Peek().Add(currentKey.Trim(), null);
                        }

                        stack.Pop();
                        currentKey = string.Empty;
                        break;
                    case ',':
                        if (!string.IsNullOrWhiteSpace(currentKey))
                        {
                            // if currentKey has a value we need to enter it
                            // before moving onto the next key after the ",".
                            stack.Peek().Add(currentKey.Trim(), null);
                        }

                        currentKey = string.Empty;
                        break;
                    default:
                        // if no other case was hit, we need to keep building 
                        // the key until we hit a char to break on
                        currentKey += ch;
                        break;
                }
            }

            // if currentKey has a value at this point, we need to add it prior to 
            // returning the parsed values.
            if (!string.IsNullOrWhiteSpace(currentKey))
            {
                stack.Peek().Add(currentKey.Trim(), null);
            }

            return result;
        }

        /// <summary>
        /// Return a formatted string based on the parsed structure from .ParseString()
        /// </summary>
        /// <param name="structure"></param>
        /// <param name="sort"></param>
        /// <param name="indentLevel"></param>
        /// <returns></returns>
        public static string PrintStructure(Dictionary<string, object?> structure, bool sort = false, int indentLevel = 0)
        {
            string output = string.Empty;

            // Sort the keys if needed
            var keys = sort ?
                structure.Keys.OrderBy(key => key).ToList() :
                structure.Keys.ToList();

            foreach (var key in keys)
            {
                // build the output
                output += $"{new string(' ', indentLevel * 2)}- {key}\n";

                if (structure[key] is Dictionary<string, object> subDict)
                {
                    // if they key contains a dictionary of its own, we need to drill into it
                    // and print those values also and add them to the output
                    output += PrintStructure(subDict, sort, indentLevel + 1);
                }
            }

            return output;
        }
    }
}
