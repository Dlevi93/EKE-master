﻿var s, g,
    Main = {

        settings: {
            gridContainer: $(".elem-grid"),
            gridContainerCat: $(".category-grid"),
            addButton: $(".btn-bitbucket"),
        },

        mvcgrid: {
            grid: $('.mvc-grid'),
        },

        initElem: function () {
            s = this.settings;
            g = this.mvcgrid;
            this.initPage();
            this.bindUIActions();
        },

        initCategory: function () {
            s = this.settings;
            g = this.mvcgrid;
            this.initPageCat();
            this.bindUIActions();
        },

        initTag: function () {
            s = this.settings;
            g = this.mvcgrid;
            this.initPageTag();
        },

        initPage: function () {
            Main.sideBarActive(".f13")
            $(".select2").select2();
            $('.mvc-grid').mvcgrid({
                reloadStarted: function (grid) {
                    Main.loadingOverlay(true, s.gridContainer)
                },
                reloadEnded: function (grid) {
                    Main.loadingOverlay(false, s.gridContainer)
                    Main.initXEditElem();
                    Main.initCoverUploader();
                },
                reloadFailed: function (grid, result) {
                    Main.loadingOverlay(false, s.gridContainer)
                },
            });

        },

        initXEditCategory: function () {
            $('.categoryNameCheck').editable({
                params: function (params) {
                    params["__RequestVerificationToken"] = $('[name="__RequestVerificationToken"]').val();
                    return params;
                }
            });
        },

        initXEditElem: function () {
            $('.elemTitleCheck').editable({
                params: function (params) {
                    params["__RequestVerificationToken"] = $('[name="__RequestVerificationToken"]').val();
                    return params;
                }
            });

            $('.elemAuthorCheck').editable({
                params: function (params) {
                    params["__RequestVerificationToken"] = $('[name="__RequestVerificationToken"]').val();
                    return params;
                }
            });

            $('.elemDescriptionCheck').editable({
                params: function (params) {
                    params["__RequestVerificationToken"] = $('[name="__RequestVerificationToken"]').val();
                    return params;
                }
            });

            $('.elemVisibleCheck').editable({
                source: [
                    { value: 'True', text: 'Igen' },
                    { value: 'False', text: 'Nem' },
                ],
                params: function (params) {
                    params["__RequestVerificationToken"] = $('[name="__RequestVerificationToken"]').val();
                    return params;
                }
            });

            $('.elemCategoryCheck').editable({
                source: "/Main/GetCategories",
                params: function (params) {
                    params["__RequestVerificationToken"] = $('[name="__RequestVerificationToken"]').val();
                    return params;
                },
                sourceCache: true,
            });

            $('.elemDateCheck').editable({
                format: 'YYYY-MM-DD',
                viewformat: 'YYYY.MM.DD',
                template: 'YYYY / MMMM / D',
                combodate: {
                    minYear: 1800,
                    maxYear: 2017,
                    minuteStep: 1
                },
                params: function (params) {
                    params["__RequestVerificationToken"] = $('[name="__RequestVerificationToken"]').val();
                    return params;
                },
            });
        },

        bindUIActions: function () {
            s.addButton.on("click", function () {
                Main.loadingOverlay(true);
                Main.createElemPartial($(this));
                Main.loadingOverlay(false);
            });
        },

        sideBarActive: function (elem) {
            $(".active").removeClass("active");
            $("" + elem + "").addClass("active");
        },

        loadingOverlay: function (show, elem) {
            if (elem === undefined) {
                if (show) {
                    $.LoadingOverlay("show")
                } else {
                    $.LoadingOverlay("hide")
                }
            } else {
                if (show) {
                    elem.LoadingOverlay("show", "'" + elem + "'")
                } else {
                    elem.LoadingOverlay("hide", "'" + elem + "'")
                }
            }
        },

        addSuccess: function () {
            $('.mvc-grid').mvcgrid({
                reload: true,
            });
        },

        createElemPartial: function (elem) {
            $.ajax({
                url: '/Main/CreateElemPartial',
                dataType: 'html',
                success: function (data) {
                    $('.add-elem').html(data);
                    CKEDITOR.replace('editor', {
                        height: 300,
                    });
                    $(".select2").select2();
                    $("#image-uploader").fileinput({
                        language: "hu",
                        showUpload: false,
                    });
                },
            });
        },

        initCoverUploader: function () {
            $(".cover-uploader").fileinput({
                language: "hu",
                showUpload: true,
                allowedFileExtensions: ["jpg", "png"],
                uploadUrl: "/Main/UploadCover",
                uploadExtraData: function (previewId, index) {
                    var elem = $(this)[0].$btnFile[0];
                    return {
                        __RequestVerificationToken: $('[name="__RequestVerificationToken"]').val(),
                        id: $(elem).find(".cover-uploader").first().data('id'),
                    };
                },
                uploadAsync: false,
            });
        },
    };