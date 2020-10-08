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

fetch('https://localhost:5001/api/CandleStick/BTCUSDT')
    .then(res => res.json())
    .then(data => {
        console.log(data)
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
    })
    .catch(err => log(err))

fetch('https://localhost:5001/api/CandleStick/BTCUSDT/15')
    .then(res => res.json())
    .then(data => {
        console.log(data)
        const cdata = data.map(d => {
            return {
                time: d.time,
                position: 'belowBar',
                color: d.color,
                shape: 'arrowUp',
                text: 'LOW',
            }
        });
        candleSeries.setMarkers(cdata);
    })
    .catch(err => log(err))


