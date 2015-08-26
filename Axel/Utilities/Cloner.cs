using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Axel.Utilities
{
    /// <summary>
    /// Cloner utility class. Clones object and associated members
    /// </summary>
    public static class Cloner
    {
        /// <summary>
        /// Clones source object. Type must be serializable.
        /// </summary>
        /// <typeparam name="T">class type being cloned</typeparam>
        /// <param name="source">object to clone</param>
        /// <returns>new cloned object</returns>
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }
  
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();

            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
