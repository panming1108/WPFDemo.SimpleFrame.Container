using EmitMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Infra.Mapping
{
    public class MapperUnit
    {
        /// <summary>
        /// 单个实体映射转化
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public TDestination ObjectMap<TSource, TDestination>(TSource source)
            where TSource : class
            where TDestination : class
        {
            ObjectsMapper<TSource, TDestination> mapper = ObjectMapperManager.DefaultInstance.GetMapper<TSource, TDestination>();
            return mapper.Map(source);
        }


        /// <summary>
        /// 集合映射转化
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public IList<TDestination> IEnumerableMap<TSource, TDestination>(IEnumerable<TSource> source)
            where TSource : class
            where TDestination : class
        {
            List<TDestination> destination = new List<TDestination>();
            foreach (var item in source)
            {
                ObjectsMapper<TSource, TDestination> mapper = ObjectMapperManager.DefaultInstance.GetMapper<TSource, TDestination>();
                destination.Add(mapper.Map(item));
            }
            return destination;
        }
    }
}
