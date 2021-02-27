const log = console.log;

$(document).ready(function () {

    $('#ticker1').click(function (event) {
        event.preventDefault();
        getAllData();
    });

    $('#binance_usdt').click(function(event) {
        getSymbols();
    });
});


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

chart.applyOptions({
    crosshair: {
        mode: 0,
    },
});

function getSymbols() {

    fetch(`https://localhost:5001/api/CandleStick/GetSymbols`)
        .then(res => res.json())
        .then(data => {
            console.log(JSON.stringify(data, null, '\t'));

            list = $('#binance_symbols');

            for (i = 0; i < data.length; i++) {
                let x = `
                    <a href="#" class="list-group-item list-group-item-action">
                        ${data[i].name}
                    </a>
                `;
                list.append(x);
            }
        })
        .catch(err => log(err))
}

function getPlanetaryLines(planet, currentPrice, from) {

    fetch(`https://localhost:5001/api/PlanetaryLines/GetPlanetaryLines?planet=${planet}&currentPrice=${currentPrice}&from=${from}&timeFrame=4h&increment=1000`)
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

function getW24Lines() {

    var maxValue = data[data.length - 1].high * 2
    fetch(`https://localhost:5001/api/PriceLevels/GetW24PriceLevels?maxValue=${maxValue}&increment=1000`)
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
}

function getAllData() {
    console.log('fetching data')

    fetch('https://localhost:5001/api/CandleStick/GetAllData?symbol=BTCUSDT&timeFrame=4h')
        .then(res => res.json())
        .then(data => {

            candleSeries.setData(data);

        })
        .catch(err => log(err))
}





