using System;
using System.Collections.Generic;

namespace QuickGenerate.Writing
{
    public class ObjectWriter
    {
        private readonly PrimitivesWriter primitivesWriter = new PrimitivesWriter();
        private readonly List<object> objectCounter = new List<object>();
        public void Write(IStream stream, object somethingToWrite)
        {
            Write(stream, somethingToWrite, 1);
        }

        public void Write(IStream stream, object somethingToWrite, int level)
        {
            objectCounter.Add(somethingToWrite);
            stream.Write(string.Format("#{0} : {1}.", objectCounter.Count, somethingToWrite.GetType().Name));
            stream.WriteLine();
            foreach (var propertyInfo in somethingToWrite.GetType().GetProperties(DomainGenerator.FlattenHierarchyBindingFlag))
            {
                level.Times(() => stream.Write("    "));
                stream.Write(string.Format("{0} = ", propertyInfo.Name));
                if (primitivesWriter.IsMatch(propertyInfo.PropertyType))
                {
                    primitivesWriter.Write(stream, propertyInfo.PropertyType, propertyInfo.GetValue(somethingToWrite, null));
                    stream.WriteLine();
                    continue;
                }
                //try
                //{
                //    var value = propertyInfo.GetValue(somethingToWrite, null);
                //    if (objectCounter.Contains(value))
                //    {
                //        stream.Write(string.Format("#{0} : {1}.", objectCounter.IndexOf(value) + 1, propertyInfo.PropertyType.Name));
                //        stream.WriteLine();
                //        continue;
                //    }
                //    Write(stream, value, ++level);
                //}
                //catch (Exception)
                //{
                //    stream.Write(string.Format("#dunno : {0}.", propertyInfo.PropertyType.Name));
                //    stream.WriteLine();
                //    continue;
                //}
            }
        }
    }
}