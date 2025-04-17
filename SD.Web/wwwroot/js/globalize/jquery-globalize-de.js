$.when(
$.getJSON("/lib/cldr-data/supplemental/likelySubtags.json"),
$.getJSON("/lib/cldr-data/supplemental/numberingSystems.json"),
$.getJSON("/lib/cldr-data/supplemental/timeData.json"),
$.getJSON("/lib/cldr-data/supplemental/weekData.json"),
$.getJSON("/lib/cldr-data/main/de/numbers.json"),
$.getJSON("/lib/cldr-data/main/de/ca-gregorian.json"),
$.getJSON("/lib/cldr-data/main/de/timeZoneNames.json"),
).then(function () {
    console.log("start slicing");
return [].slice.apply(arguments, [0]).map(function (result) {
    console.log("slicing done");
return result[0];
    });
}).then(Globalize.load).then(function () {
    Globalize.locale("de");
console.log("Locale set to de");
}).then(console.log("LOADED EVERYTHING"));
