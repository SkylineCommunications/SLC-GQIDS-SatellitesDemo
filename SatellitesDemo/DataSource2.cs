using Skyline.DataMiner.Analytics.GenericInterface;

namespace Demo
{
	[GQIMetaData(Name = "Satellites Demo v2: with logging")]
    public sealed class DataSource2 : IGQIDataSource, IGQIOnInit, IGQIOnDestroy
    {
		// Define a field to store the logger
		private IGQILogger _logger;

        public OnInitOutputArgs OnInit(OnInitInputArgs args)
        {
            // Store the logger
            _logger = args.Logger;
            _logger.Information("Initialized");

            return default;
        }

        public GQIColumn[] GetColumns()
        {
            _logger.Information("Defining columns...");

            // Define the columns of the data source
            var nameColumn = new GQIStringColumn("Name");
            var latitudeColumn = new GQIDoubleColumn("Latitude");
            var longitudeColumn = new GQIDoubleColumn("Longitude");

            // Give the columns to the GQI framework
            return new GQIColumn[]
            {
                nameColumn,
                latitudeColumn,
                longitudeColumn,
            };
        }

        public GQIPage GetNextPage(GetNextPageInputArgs args)
        {
            _logger.Information("Getting the next page...");

			// Define some static data
			// Each row represents a satellite
			var row1 = new GQIRow(new[]
            {
                new GQICell { Value = "Galaxy 17" },
                new GQICell { Value = 0.0 },
                new GQICell { Value = -91.0 },
            });
            var row2 = new GQIRow(new[]
            {
                new GQICell { Value = "Thor 7" },
                new GQICell { Value = 0.0 },
                new GQICell { Value = 0.8 },
            });
            var row3 = new GQIRow(new[]
            {
                new GQICell { Value = "Yamal 201" },
                new GQICell { Value = 0.0 },
                new GQICell { Value = 90.0 },
            });

            var rows = new[]
            {
                row1,
                row2,
                row3,
            };

			// Give the rows as a single page to the GQI framework
			return new GQIPage(rows);
        }

        public OnDestroyOutputArgs OnDestroy(OnDestroyInputArgs args)
        {
            _logger.Information("Destroyed");
            return default;
        }
    }
}
