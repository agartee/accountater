﻿export function renderMonthlyChart(monthlyCategorySpending, monthlyIncome, tags) {
    const incomeLookup = (month, year) => {
        return monthlyIncome.find(i => i.month === month && i.year === year)?.amount ?? 0;
    };

    const getMonthKey = (month, year) => {
        const date = new Date(year, month - 1);
        return `${date.toLocaleString('en-US', { month: 'short' })} ${year}`;
    };

    const svg = d3.select("#chart"),
        width = 1000,
        height = 600,
        margin = { top: 20, right: 20, bottom: 40, left: 80 };

    const grouped = d3.groups(monthlyCategorySpending, d => getMonthKey(d.month, d.year));
    const months = grouped.map(([key]) => key);

    const fullData = grouped.flatMap(([monthKey, rows]) => {
        const [monthAbbr, year] = monthKey.split(' ');
        const date = new Date(`${monthAbbr} 1, ${year}`);
        const month = date.getMonth() + 1;

        const spent = d3.sum(rows, d => d.amount);
        const income = incomeLookup(month, +year);
        const remainder = income - spent;

        const extraCategory = remainder >= 0
            ? { monthName: monthKey, category: 'Remaining', amount: remainder }
            : { monthName: monthKey, category: 'Overspending', amount: -remainder };

        const reshaped = rows.map(r => ({
            monthName: monthKey,
            category: r.category,
            amount: r.amount
        }));

        return [...reshaped, extraCategory];
    });

    const categories = Array.from(new Set(fullData.map(d => d.category)));

    const x = d3.scalePoint()
        .domain(months)
        .range([margin.left, width - margin.right])
        .padding(0.5);

    const spacing = x(months[1]) - x(months[0]);
    const barWidth = spacing * 0.6;

    const y = d3.scaleLinear().range([height - margin.bottom, margin.top]);

    const stacked = d3.groups(fullData, d => d.monthName).flatMap(([monthName, values]) => {
        const sorted = values
            .slice()
            .sort((a, b) => {
                const orderA = tags[a.category]?.order ?? 999;
                const orderB = tags[b.category]?.order ?? 999;
                if (orderA !== orderB) return orderB - orderA;
                return d3.descending(a.category, b.category);
            });

        let y0 = 0;
        return sorted.map(d => ({
            ...d,
            y0,
            y1: y0 += d.amount,
            monthX: x(monthName)
        }));
    });

    y.domain([0, d3.max(stacked, d => d.y1)]);

    for (let i = 0; i < months.length - 1; i++) {
        const groupA = stacked.filter(d => d.monthName === months[i]);
        const groupB = stacked.filter(d => d.monthName === months[i + 1]);

        categories.forEach(cat => {
            const a = groupA.find(d => d.category === cat);
            const b = groupB.find(d => d.category === cat);
            if (a && b) {
                const path = d3.path();
                const midX = (a.monthX + b.monthX) / 2;
                path.moveTo(a.monthX + barWidth / 2, y(a.y0));
                path.bezierCurveTo(midX, y(a.y0), midX, y(b.y0), b.monthX - barWidth / 2, y(b.y0));
                path.lineTo(b.monthX - barWidth / 2, y(b.y1));
                path.bezierCurveTo(midX, y(b.y1), midX, y(a.y1), a.monthX + barWidth / 2, y(a.y1));
                path.closePath();

                svg.append("path")
                    .attr("class", "ribbon")
                    .attr("fill", tags[cat].color)
                    .attr("d", path);
            }
        });
    }

    svg.selectAll("rect")
        .data(stacked)
        .join("rect")
        .attr("class", "bar")
        .attr("x", d => d.monthX - barWidth / 2)
        .attr("width", barWidth)
        .attr("y", d => y(d.y1))
        .attr("height", d => y(d.y0) - y(d.y1))
        .attr("fill", d => tags[d.category].color);

    svg.selectAll(".label")
        .data(stacked.filter(d => d.amount > 0))
        .join("text")
        .attr("class", "label")
        .attr("x", d => d.monthX)
        .attr("y", d => y((d.y0 + d.y1) / 2))
        .text(d => `$${d.amount.toFixed(0)}`);

    svg.append("g")
        .attr("transform", `translate(0,${height - margin.bottom})`)
        .call(d3.axisBottom(x));

    const legend = d3.select("#legend");

    const sortedCategories = categories
        .filter(c => c !== 'Remaining' && c !== 'Overspending')
        .sort((a, b) => {
            const orderA = tags[a]?.order ?? 999;
            const orderB = tags[b]?.order ?? 999;
            if (orderA !== orderB) return orderA - orderB;
            return a.localeCompare(b);
        });

    sortedCategories.push('Remaining', 'Overspending');

    sortedCategories.forEach(cat => {
        legend.append("div")
            .attr("class", "legend-item")
            .html(`
                <div class="legend-color" style="background:${tags[cat].color};"></div>
                <span>${cat}</span>
            `);
    });
}
