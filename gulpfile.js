var gulp = require('gulp'),
cleanCSS = require('gulp-clean-css'),
concat = require('gulp-concat');

gulp.task('minify', function () {
    gulp.src('Cloud-based-editor-VLN-2/Content/Stylesheets/*.css')
        .pipe(cleanCSS())
        .pipe(concat('AllStylesConcat.min.css'))
        .pipe(gulp.dest('dest'));
});