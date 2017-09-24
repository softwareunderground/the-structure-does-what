# The structure does _WHAT!?_

Some code from a project built during the 
[2017 Geophysics Hackathon](https://agilescientific.com/blog/2017/7/24/newsflash-the-geophysics-hackathon-is-back).

## Team

- Nate Suurmeyer
- Nanne Hemstra
- Chris Ennen
- Jacob Foshee

## `geocells` CLI

The `geocells` directory contains a tool that can do a few things:

- Generates random locations, we use as well locations
- Computes a "distance field" horizon where each cell has a distance-to-nearest-well
- Also computes a "score" for each horizon sample proportional to porosity and inversely proportional to depth and distance-to-well

We treat Inline as X and Crossline as Y.

### Building

Requires the [.NET Core](https://www.microsoft.com/net/core) tools.

```
cd geocells
dotnet build
```

### Running

The primary input is a horizon which is tab-delimited with a header like the following:

```
"Inline"	"Crossline"	"Z"	"Porosity"	"Amplitude"
```

To generate 5 well locations in the inline/crossline space of the horizon:

```
dotnet run -- gen well_locations.csv -h horizon.txt -n 5
```

To calculate distances to wells and very rough metrics of well location goodness:

```
dotnet run -- crunch horizon.txt well_locations.csv output_horizon.csv
```

## Credits

The included sample data is courtesy of the [dGB Open Seismic Repository](http://www.opendtect.org/osr/). 
We used horizon FS8 from 
[Netherlands Offshore F3 Block](http://www.opendtect.org/osr/pmwiki.php/Main/NetherlandsOffshoreF3BlockComplete4GB).

