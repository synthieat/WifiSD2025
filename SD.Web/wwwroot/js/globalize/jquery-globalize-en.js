$.when(
    $.getJSON("/lib/cldr-data/supplemental/likelySubtags.json"),
    $.getJSON("/lib/cldr-data/supplemental/numberingSystems.json"),
    $.getJSON("/lib/cldr-data/supplemental/timeData.json"),
    $.getJSON("/lib/cldr-data/supplemental/weekData.json"),
    $.getJSON("/lib/cldr-data/main/en/numbers.json"),
    $.getJSON("/lib/cldr-data/main/en/ca-gregorian.json"),
    $.getJSON("/lib/cldr-data/main/en/timeZoneNames.json"),
).then(function () {
    console.log("start slicing");
    return [].slice.apply(arguments, [0]).map(function (result) {
        console.log("slicing done");
        return result[0];
    });
}).then(Globalize.load).then(function () {
    Globalize.locale("en");
    console.log("Locale set to en");
}).then(console.log("LOADED EVERYTHING"));
