window.cloudCharts = (() => {
    let instancesChart = null;
    let usageChart = null;

    function destroyChart(chart) {
        if (chart) {
            chart.destroy();
        }
    }

    function renderInstancesChart(running, stopped) {
        const canvas = document.getElementById("instancesChart");
        if (!canvas) return;

        const ctx = canvas.getContext("2d");
        destroyChart(instancesChart);

        instancesChart = new Chart(ctx, {
            type: "doughnut",
            data: {
                labels: ["Running", "Stopped"],
                datasets: [{
                    data: [running, stopped],
                    backgroundColor: ["#22c55e", "#ef4444"],
                    borderWidth: 0,
                    hoverOffset: 10
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                animation: {
                    duration: 900,
                    easing: "easeOutQuart"
                },
                plugins: {
                    legend: {
                        labels: {
                            color: "#cbd5e1"
                        }
                    }
                }
            }
        });
    }

    function renderUsageChart(cpuUsage, memoryUsage) {
        const canvas = document.getElementById("usageChart");
        if (!canvas) return;

        const ctx = canvas.getContext("2d");
        destroyChart(usageChart);

        usageChart = new Chart(ctx, {
            type: "bar",
            data: {
                labels: ["CPU Usage", "Memory Usage"],
                datasets: [{
                    label: "Average %",
                    data: [cpuUsage, memoryUsage],
                    backgroundColor: ["#3b82f6", "#6366f1"],
                    borderRadius: 10,
                    borderSkipped: false
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                animation: {
                    duration: 900,
                    easing: "easeOutQuart"
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        max: 100,
                        ticks: {
                            color: "#cbd5e1"
                        },
                        grid: {
                            color: "rgba(148, 163, 184, 0.12)"
                        }
                    },
                    x: {
                        ticks: {
                            color: "#cbd5e1"
                        },
                        grid: {
                            display: false
                        }
                    }
                },
                plugins: {
                    legend: {
                        labels: {
                            color: "#cbd5e1"
                        }
                    }
                }
            }
        });
    }

    return {
        renderInstancesChart,
        renderUsageChart
    };
})();