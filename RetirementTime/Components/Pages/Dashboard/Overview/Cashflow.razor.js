import * as d3 from "https://cdn.jsdelivr.net/npm/d3@7/+esm";
import { sankey, sankeyLinkHorizontal, sankeyLeft, sankeyRight, sankeyCenter, sankeyJustify } from "https://cdn.jsdelivr.net/npm/d3-sankey@0.12/+esm";

let _uidCounter = 0;
function uid(prefix) {
    const id = `${prefix}-${++_uidCounter}`;
    return { id, href: `#${id}` };
}

export function render(containerId, data, nodeAlign = "sankeyJustify", linkColor = "source-target") {
    const container = document.getElementById(containerId);
    if (!container) return;

    const alignments = { sankeyLeft, sankeyRight, sankeyCenter, sankeyJustify };
    const align = alignments[nodeAlign] ?? sankeyJustify;

    const format = d3.format(",.0f");
    const width = container.clientWidth || 928;
    const height = 600;

    const sankeyGen = sankey()
        .nodeId(d => d.name)
        .nodeAlign(align)
        .nodeWidth(15)
        .nodePadding(10)
        .extent([[1, 5], [width - 1, height - 5]]);

    const { nodes, links } = sankeyGen({
        nodes: data.nodes.map(d => Object.assign({}, d)),
        links: data.links.map(d => Object.assign({}, d))
    });

    const color = d3.scaleOrdinal(d3.schemeCategory10);

    const svg = d3.create("svg")
        .attr("width", width)
        .attr("height", height)
        .attr("viewBox", [0, 0, width, height])
        .attr("style", "max-width: 100%; height: auto; font: 13px sans-serif;");

    // Nodes
    const rect = svg.append("g")
        .attr("stroke", "#000")
        .selectAll()
        .data(nodes)
        .join("rect")
        .attr("x", d => d.x0)
        .attr("y", d => d.y0)
        .attr("height", d => d.y1 - d.y0)
        .attr("width", d => d.x1 - d.x0)
        .attr("fill", d => color(d.category ?? d.name));

    rect.append("title")
        .text(d => `${d.name}\n$${format(d.value)}`);

    // Links
    const link = svg.append("g")
        .attr("fill", "none")
        .attr("stroke-opacity", 0.5)
        .selectAll()
        .data(links)
        .join("g")
        .style("mix-blend-mode", "multiply");

    if (linkColor === "source-target") {
        const gradient = link.append("linearGradient")
            .attr("id", d => (d.uid = uid("link")).id)
            .attr("gradientUnits", "userSpaceOnUse")
            .attr("x1", d => d.source.x1)
            .attr("x2", d => d.target.x0);
        gradient.append("stop")
            .attr("offset", "0%")
            .attr("stop-color", d => color(d.source.category ?? d.source.name));
        gradient.append("stop")
            .attr("offset", "100%")
            .attr("stop-color", d => color(d.target.category ?? d.target.name));
    }

    link.append("path")
        .attr("d", sankeyLinkHorizontal())
        .attr("stroke", linkColor === "source-target" ? d => `url(#${d.uid.id})`
            : linkColor === "source" ? d => color(d.source.category ?? d.source.name)
            : linkColor === "target" ? d => color(d.target.category ?? d.target.name)
            : linkColor)
        .attr("stroke-width", d => Math.max(1, d.width));

    link.append("title")
        .text(d => `${d.source.name} → ${d.target.name}\n$${format(d.value)}`);

    // Labels
    svg.append("g")
        .selectAll()
        .data(nodes)
        .join("text")
        .attr("x", d => d.x0 < width / 2 ? d.x1 + 6 : d.x0 - 6)
        .attr("y", d => (d.y1 + d.y0) / 2 - 7)
        .attr("dy", "0.35em")
        .attr("text-anchor", d => d.x0 < width / 2 ? "start" : "end")
        .attr("font-size", "13px")
        .attr("font-weight", "600")
        .attr("font-family", "Inter, Manrope, sans-serif")
        .text(d => d.name);

    svg.append("g")
        .selectAll()
        .data(nodes)
        .join("text")
        .attr("x", d => d.x0 < width / 2 ? d.x1 + 6 : d.x0 - 6)
        .attr("y", d => (d.y1 + d.y0) / 2 + 9)
        .attr("dy", "0.35em")
        .attr("text-anchor", d => d.x0 < width / 2 ? "start" : "end")
        .attr("font-size", "12px")
        .attr("font-family", "Inter, Manrope, sans-serif")
        .attr("fill", "#7a6e65")
        .text(d => "$" + format(d.value));

    container.innerHTML = "";
    container.appendChild(svg.node());
}

export function renderBarChart(containerId, yearlyCashFlows, labelIncome, labelExpenses, labelSavings) {
    const container = document.getElementById(containerId);
    if (!container || !yearlyCashFlows || yearlyCashFlows.length === 0) return;

    function doRender() {
        const ec = window.echarts;
        if (!ec) return;

        const ages     = yearlyCashFlows.map(d => d.year);
        const incomes  = yearlyCashFlows.map(d => Math.round(d.totalIncome));
        const expenses = yearlyCashFlows.map(d => Math.round(d.totalExpenses));

        const chart = ec.init(container, null, { renderer: 'svg' });

        chart.setOption({
            tooltip: {
                trigger: 'axis',
                axisPointer: { type: 'shadow' },
                formatter: params => {
                    let s = `<b>${params[0].axisValue}</b><br/>`;
                    params.forEach(p => {
                        s += `${p.marker}${p.seriesName}: $${p.value.toLocaleString()}<br/>`;
                    });
                    return s;
                }
            },
            legend: {
                data: [labelIncome, labelExpenses],
                textStyle: { fontFamily: 'Inter, Manrope, sans-serif', fontSize: 13 }
            },
            grid: { left: '3%', right: '4%', bottom: '3%', containLabel: true },
            xAxis: [{
                type: 'category',
                data: ages,
                name: 'Age',
                nameLocation: 'middle',
                nameGap: 30,
                axisLabel: { fontFamily: 'Inter, Manrope, sans-serif', fontSize: 12 }
            }],
            yAxis: [{
                type: 'value',
                axisLabel: {
                    fontFamily: 'Inter, Manrope, sans-serif',
                    fontSize: 12,
                    formatter: v => '$' + (v >= 1000 ? (v / 1000).toFixed(0) + 'k' : v)
                }
            }],
            color: ['#9A3412', '#334155'],
            series: [
                {
                    name: labelIncome,
                    type: 'bar',
                    emphasis: { focus: 'series' },
                    data: incomes,
                    barMaxWidth: 20
                },
                {
                    name: labelExpenses,
                    type: 'bar',
                    emphasis: { focus: 'series' },
                    data: expenses,
                    barMaxWidth: 20
                }
            ]
        });

        window.addEventListener('resize', () => chart.resize());
    }

    if (window.echarts) {
        doRender();
    } else {
        const script = document.createElement('script');
        script.src = 'https://cdn.jsdelivr.net/npm/echarts@5/dist/echarts.min.js';
        script.onload = doRender;
        document.head.appendChild(script);
    }
}

