USE CycleFinderDB;
GO
BULK INSERT Ephemeris FROM 'C:\Users\Zsolt\data\trading\data\Ephem_Daily_2010_2030_planets.csv'
   WITH (
      FIELDTERMINATOR = ',',
      ROWTERMINATOR = '\n'
);
GO