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
        fetch('https://localhost:5001/api/CandleStick/GetLowsWithTurns?symbol=BTCUSDT&numberoflows=5')
            .then(res => res.json())
            .then(data => {
                console.log(data)
                const lows = data.map(d => {
                    return {
                        time: d.time,
                        position: d.position,
                        color: d.color,
                        shape: d.shape,
                        text: d.text,
                    }
                });

                //Get highs
                fetch('https://localhost:5001/api/CandleStick/GetHighsWithTurns?symbol=BTCUSDT&numberofhighs=5')
                    .then(res => res.json())
                    .then(data => {
                        console.log(data)
                        const highs = data.map(d => {
                            return {
                                time: d.time,
                                position: d.position,
                                color: d.color,
                                shape: d.shape,
                                text: d.text,
                            }
                        });

                        candleSeries.setMarkers(lows.concat(highs));
                    })
                    .catch(err => log(err))
            })
            .catch(err => log(err))

    })
    .catch(err => log(err))




