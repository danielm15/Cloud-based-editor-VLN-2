var gulp = require('gulp'),
minifyCSS = require('gulp-clean-css'),
minify = require('gulp-uglify'),
concat = require('gulp-concat');


gulp.task('minifyCSS', function () {
    gulp.src('Cloud-based-editor-VLN-2/Content/Stylesheets/*.css')
        .pipe(minifyCSS())
        .pipe(concat('AllStylesConcat.min.css'))
        .pipe(gulp.dest('dest'));
});


gulp.task('minifyJS', function () {
    gulp.src(['Cloud-based-editor-VLN-2/Scripts/ProjectOverview.js', 'Cloud-based-editor-VLN-2/Scripts/documentList.js',
    'Cloud-based-editor-VLN-2/Scripts/FileOverview.js', 'Cloud-based-editor-VLN-2/Scripts/notifications.js'])
        .pipe(minify())
        .pipe(concat('CustomScripts.min.js'))
        .pipe(gulp.dest('dest'));
});