const log = console.log;

const chartProterties = {
    width:1500,
    height:600,
    timeScale:{
        timeVisible:true,
        secondsVisible:false,
    }
}

const domElement = document.getElementById('tvchart');
const chart = LightweightCharts.createChart(domElement, chartProterties);
const candleSeries = chart.addCandlestickSeries();

chart.applyOptions({
    crosshair: {
        mode: 0,
    },
});


//Get all data
fetch('https://localhost:5001/api/CandleStick/GetAllData?symbol=BTCUSDT')
    .then(res => res.json())
    .then(data => {
        const cdata = data.map(d => {
            return {
                time: d.time,
                open: d.open,
                high: d.high,
                low: d.low,
                close: d.close
            }
        });
        candleSeries.setData(cdata);

        //Get lows
        fetch('https://localhost:5001/api/CandleStickMarker/GetLowsWithPlanetPositions?symbol=BTCUSDT&planet=mercury&limit=15')
            .then(res => res.json())
            .then(data => {
                console.log(JSON.stringify(data, null, '\t'));

                /*var i;
                for (i = 0; i < whitespaces.length; i++) {
                    candleSeries.update({ time: whitespaces[i].time })
                }*/

                const lows = data.filter(l => !l.isInTheFuture).map(d => {
                    return {
                        time: d.time,
                        position: d.position,
                        color: d.color,
                        shape: d.shape,
                        text: d.text,
                    }
                });

                candleSeries.setMarkers(lows);
            })
            .catch(err => log(err))
    })
    .catch(err => log(err))




