namespace Examples.RailwayOrientedProgramming.Logic.Common
{
    public  class Repository<T>
        where T : Entity 
    {
        

      

        public Maybe<T> GetById(long id)
        {
            return null;
            // return T
        }

        public void Save(T entity)
        {
            // Save method
        }
    }
}