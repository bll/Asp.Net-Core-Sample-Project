/// <binding AfterBuild='default' />

var gulp = require('gulp');
var uglify = require('gulp-uglify');
var concat = require('gulp-concat');

gulp.task("minify", function () {
    return gulp.src("wwwroot/js/**/*.js") // wwwroot js ve alt dizinleri
        .pipe(uglify()) // tüm dosyaları alır ve minify eder
        .pipe(concat("dotnetcore.min.js")) // sıkıştırılan dosyalarımızı vereceğimiz isimle kaydeder.
        .pipe(gulp.dest("wwwroot/dist")); // yeni bir klasör aşıp sıkıştırarak yarattığı dosyayı buraya kaydeder
         
});
gulp.task('default',["minify"]);