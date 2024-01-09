using AutoMapper;

namespace SparkSwim.Core.Mapping
{
    public interface IMapWith<T>
    {
        void Mapping(Profile profile);
    }
}
