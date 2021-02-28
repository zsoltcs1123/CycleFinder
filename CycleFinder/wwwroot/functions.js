const log = console.log;
var currentChartData = [];
var w24l_on = false;
var priceLines = [];

Number.prototype.countDecimals = function () {
    if (Math.floor(this.valueOf()) === this.valueOf()) return 0;
    return this.toString().split(".")[1].length || 0;
}

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
    width:1200,
    height:510,
    timeScale:{
        timeVisible:true,
        secondsVisible:false,
    }
}

const domElement = document.getElementById('tvchart');
const chart = LightweightCharts.createChart(domElement, chartProterties);
var candleSeries = chart.addCandlestickSeries();
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
                list.append('<a href="#" id="' + data[i].name + '" onclick ="onSymbolClick(event)"' 
                    + ' class="list-group-item list-group-item-action">' + data[i].name + '</a>');
            }
        })
        .catch(err => log(err))
}

function onSymbolClick(e) {
    e = e || window.event;
    var target = e.target || e.srcElement;
    console.log(target.id);
    getAllData(target.id)
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

    if (w24l_on) {
        for (i = 0; i < priceLines.length; i++) {
            candleSeries.removePriceLine(priceLines[i]);
        }
        w24l_on = false;
        return;
    }

    var currentPrice = currentChartData[currentChartData.length - 1].high
    var maxValue = currentPrice * 3

    var dec = currentPrice.countDecimals();
/*var inc = dec == 1 ? Math.pow(10, dec - 4) : dec == 2 ? Math.pow(10, dec - 4) : dec == 3 ? Math.pow(10, dec - 3) : dec == 4 ? Math.pow(10, dec - 2) : Math.pow(10, dec - 2)*/
    inc = Math.pow(10, dec - 4);
    
    fetch(`https://localhost:5001/api/PriceLevels/GetW24PriceLevels?maxValue=${maxValue}&increment=${inc}`)
        .then(res => res.json())
        .then(data => {
            console.log(JSON.stringify(data, null, '\t'));

            var i;
            for (i = 0; i < data.length; i++) {
                var pline =
                candleSeries.createPriceLine({
                    price: data[i].price,
                    color: data[i].lineColor,
                    lineWidth: data[i].lineWidth,
                    lineStyle: LightweightCharts.LineStyle.Dashed
                });
                priceLines.push(pline)

            }
            w24l_on = true;
        })
        .catch(err => log(err))
}

function getAllData(id) {
    console.log('fetching data' + id)
    chart.removeSeries(candleSeries);
    w24l_on = false;
    $("b_w24l").button('toggle')

    fetch(`https://localhost:5001/api/CandleStick/GetAllData?symbol=${id}&timeFrame=1d`)
        .then(res => res.json())
        .then(data => {

            currentChartData = data;
            candleSeries = chart.addCandlestickSeries();
            candleSeries.setData(data);

        })
        .catch(err => log(err))
}





