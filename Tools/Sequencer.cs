using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using URLShorteningService.Data;

namespace URLShorteningService.Tools
{
    public class Sequencer
    {
        private readonly IUnitOfWork _unitOfWork;
        public Sequencer(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // shared counter
        private static long _counter = 0; 
        // base58 encoding
        private static readonly string Characters = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
        private static readonly int KeyLenght = 6;

        public string GenerateKey()
        {
            //long currentValue;
            bool isUnique = false;
            string key = "";
            int retryCount = 0;
            int maxRetries = 10;

            do
            {
                if (retryCount >= maxRetries)
                    throw new Exception("Max retries creating unique key was reached :(");

                /* Atomic increment
                 * If two threads call Iterlocked.Increment at almost
                 * the same time, they will get distinct sequential values
                 * from _counter
                 */
                //currentValue = Interlocked.Increment(ref _counter);
                //key = Encode(currentValue);

                key = RandomKey();
                isUnique = !_unitOfWork.Urls.AnyByKey(key);
                retryCount++;

            } while (!isUnique);

            return key;
        }

        private static string RandomKey()
        {
            var randomBytes = new byte[KeyLenght];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return new string(randomBytes
                .Select(b => Characters[b % Characters.Length])
                .ToArray());
        }

        private static string Encode(long value)
        {
            var result = new char[KeyLenght];

            for (int i = KeyLenght - 1; i >= 0; i--)
            {
                result[i] = Characters[(int)(value % Characters.Length)];
                value /= Characters.Length;
            }

            return new string(result);
        }
    }
}
