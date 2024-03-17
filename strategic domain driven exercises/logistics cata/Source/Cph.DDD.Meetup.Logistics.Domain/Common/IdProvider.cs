namespace Cph.DDD.Meetup.Logistics.Domain.Common
{
    public class IdProvider 
    {
        struct IdCounter
        {
            public IdCounter(int seriesNumber, int currentCounter)
            {
                SeriesNumber = seriesNumber;
                CurrentNumber = currentCounter;
            }

            public readonly int SeriesNumber ;
            public int CurrentNumber { get; set; }
        }


        private static  Dictionary<Type, IdCounter> idDictionary = new Dictionary<Type,IdCounter>();

        public IdProvider()
        {
           
        }

        private static void AddIdSerie(Type type )
        {
            if ( idDictionary.ContainsKey( type ) )
                return;
            idDictionary.Add(type, new IdCounter(idDictionary.Count + 1 , 1));
        }


        public static Guid GetNextId(Type type)
        {

            if ( !idDictionary.ContainsKey( type ) )
            {
                AddIdSerie( type );
            }

            var idCounter = idDictionary[ type ];

            var id= Guid.Parse( $"{idCounter.SeriesNumber.ToString( "X8" )}-0000-0000-0000-{idCounter.CurrentNumber.ToString( "X12" )}" );
            idCounter.CurrentNumber++;

            return id;
        }
    }

}
