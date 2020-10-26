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
        
        candleSeries.setData(data);

        //get planetary lines
        fetch(`https://localhost:5001/api/PlanetaryLines/GetPlanetaryLines?planet=sa&currentPrice=${data[data.length-2].open}&from=${data[0].time}`)
            .then(res => res.json())
            .then(data => {
                console.log(JSON.stringify(data, null, '\t'));

                var i;
                for (i = 0; i < data.length; i++) {
                    const lineSeries = chart.addLineSeries();
                    lineSeries.setData(data[i].lineValues);
                }
            })
            .catch(err => log(err))


        //Get aspects
        fetch(`https://localhost:5001/api/CandleStickMarker/GetAspects?from=${cdata[0].time}&planet=su,ju`)
            .then(res => res.json())
            .then(data => {
                console.log(JSON.stringify(data, null, '\t'));

                const markers = data.filter(l => !l.isInTheFuture).map(d => {
                    return {
                        time: d.time,
                        position: d.position,
                        color: d.color,
                        shape: d.shape,
                        text: d.text,
                    }
                });

                candleSeries.setMarkers(markers);
            })
            .catch(err => log(err))
    })
    .catch(err => log(err))




