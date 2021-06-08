const log = console.log;

const chartProterties = {
    width:1800,
    height:900,
    timeScale:{
        timeVisible:true,
        secondsVisible:false,
    }
}

const domElement = document.getElementById('tvchart');
const chart = LightweightCharts.createChart(domElement, chartProterties);
const candleSeries = chart.addCandlestickSeries();
var currentMarkers = [];

function getPlanetaryLines(planet, currentPrice, from) {

    fetch(`https://localhost:5001/api/PlanetaryLines/GetPlanetaryLines?planet=${planet}&currentPrice=${currentPrice}&from=${from}&timeFrame=1d&increment=1000`)
        .then(res => res.json())
        .then(data => {
            console.log(JSON.stringify(data, null, '\t'));

            var i;
            for (i = 0; i < data.length; i++) {
                const lineSeries = chart.addLineSeries({
                    color: data[i].color,
                    lineWidth: 2,
                    priceLineVisible: false
                });
                lineSeries.setData(data[i].lineValues);
            }
        })
        .catch(err => log(err))
}

function getAspects(p1, p2, from) {

    fetch(`https://localhost:5001/api/CandleStickMarker/GetAspects?from=${from}&planet=${p1},${p2}`)
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

            //const uniqueMarkers = [...new Set(currentMarkers.concat(markers).map(item => item.time))]
            currentMarkers = (currentMarkers.concat(markers))
                .sort((a, b) => (a.time > b.time) ? 1 : ((b.time > a.time ? -1 : 0)));

            candleSeries.setMarkers(currentMarkers);
        })
        .catch(err => log(err))
}

function getLowTurns(symbol, limit) {

    fetch(`https://localhost:5001/api/CandleStickMarker/GetLowsWithTurns?symbol=${symbol}&limit=${limit}`)
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

            //const uniqueMarkers = [...new Set(currentMarkers.concat(markers).map(item => item.time))]
            currentMarkers = currentMarkers.concat(markers)

            candleSeries.setMarkers(currentMarkers);
        })
        .catch(err => log(err))
}

function getHighTurns(symbol, limit) {

    fetch(`https://localhost:5001/api/CandleStickMarker/GetHighsWithTurns?symbol=${symbol}&limit=${limit}`)
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

            //const uniqueMarkers = [...new Set(currentMarkers.concat(markers).map(item => item.time))]
            currentMarkers = currentMarkers.concat(markers)

            candleSeries.setMarkers(currentMarkers);
        })
        .catch(err => log(err))
}

function getMarkers(url) {

    fetch(url)
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

            //const uniqueMarkers = [...new Set(currentMarkers.concat(markers).map(item => item.time))]
            currentMarkers = (currentMarkers.concat(markers))
                .sort((a, b) => (a.time > b.time) ? 1 : ((b.time > a.time ? -1 : 0)));

            candleSeries.setMarkers(currentMarkers);
        })
        .catch(err => log(err))
}

chart.applyOptions({
    crosshair: {
        mode: 0,
    },
});


//Get all data
fetch('https://localhost:5001/api/CandleStick/GetAllData?symbol=XRPBTC&timeFrame=4h')
    .then(res => res.json())
    .then(data => {
        
        candleSeries.setData(data);

        candleSeries.applyOptions({
            priceFormat: {
                precision: 8,
                type: 'volume',
            },
        });

        //get planetary lines
        getPlanetaryLines('sa', data[data.length - 2].open, data[0].time)
        getPlanetaryLines('ju', data[data.length - 2].open, data[0].time)
        getPlanetaryLines('pl', data[data.length - 2].open, data[0].time)
        getPlanetaryLines('ne', data[data.length - 2].open, data[0].time)
        getPlanetaryLines('ur', data[data.length - 2].open, data[0].time)
        getPlanetaryLines('ma', data[data.length - 2].open, 1577836800)

        //Get w24 lines

        var maxValue = data[data.length - 1].high * 2
        fetch(`https://localhost:5001/api/PriceLevels/GetW24PriceLevels?maxValue=${maxValue}&increment=0.000001`)
            .then(res => res.json())
            .then(data => {
                console.log(JSON.stringify(data, null, '\t'));

                var i;
                for (i = 0; i < data.length; i++) {
                    candleSeries.createPriceLine({
                        price: data[i].price,
                        color: data[i].lineColor,
                        lineWidth: data[i].lineWidth,
                        lineStyle: LightweightCharts.LineStyle.Dashed
                    });
                }
            })
            .catch(err => log(err))

        //getLowTurns('BTCUSDT', 10)
        //getHighTurns('BTCUSDT', 10)
        //Get aspects
        //getAspects('ju', 'pl', data[0].time)
        //getAspects('me', 'sa', data[0].time)
        //getAspects('me', 'ur', data[0].time)
        //getAspects('me', 'sa', data[0].time)

        //getMarkers('https://localhost:5001/api/CandleStickMarker/GetHighsWithTurns?symbol=BTCUSDT&limit=10')
        //getMarkers('https://localhost:5001/api/CandleStickMarker/GetLowsWithTurns?symbol=BTCUSDT&limit=10')
        getMarkers(`https://localhost:5001/api/PlanetaryLines/GetW24Crossings?planet=ma&from=${data[0].time}`)
        getMarkers(`https://localhost:5001/api/PlanetaryLines/GetW24Crossings?planet=su&from=${data[0].time}`)
        getMarkers(`https://localhost:5001/api/PlanetaryLines/GetW24Crossings?planet=ve&from=${data[0].time}`)
        getMarkers(`https://localhost:5001/api/PlanetaryLines/GetW24Crossings?planet=me&from=${data[0].time}`)
    })
    .catch(err => log(err))




