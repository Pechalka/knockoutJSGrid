var AjaxGridViewModel = function (url, filterParams, sort) {
    this.rows = ko.observableArray();
    this.filterParams = ko.mapping.fromJS(filterParams);

    this.paging = {
        PageNumber: ko.observable(1),
        TotalPagesCount: ko.observable(0),
        next: function () {
            var pn = this.PageNumber();
            if (pn < this.TotalPagesCount()) this.PageNumber(pn + 1);
        },
        back: function () {
            var pn = this.PageNumber();
            if (pn > 1) this.PageNumber(pn - 1);
        }
    };

    this.sort = ko.mapping.fromJS(sort);
    this.onSorting = function (newSorting) {

    };

    this.changeSort = function (newSort) {
        if (newSort == this.sort.Field()) {
            var newDist = this.sort.Distinct() == 'asc' ? 'desc' : 'asc';
            this.sort.Distinct(newDist);
        }
        this.sort.Field(newSort);
        this.onSorting(this.sort);
    };

    ko.dependentObservable(function () {
        ko.toJS(this.filterParams);
        this.paging.PageNumber(1);
    }, this);

    ko.dependentObservable(function () {
        var data = ko.utils.unwrapObservable(this.filterParams);
        data.pageNumber = this.paging.PageNumber();
        data.sort = ko.toJS(this.sort);
        debugger;
        $.ajax({
            url: url,
            type: 'POST',
            data: ko.toJSON(data),
            context: this,
            contentType: 'application/json',
            success: function (data) {
                this.rows(data.Data);
                this.paging.PageNumber(data.Paging.PageNumber);
                this.paging.TotalPagesCount(data.Paging.TotalPagesCount);
            }
        });
        location.hash = this.paging.PageNumber();
    }, this);

    var self = this;
    Sammy(function () {
        this.get('#:page', function () {
            var page = parseInt(this.params.page, 10);
            self.paging.PageNumber(page);
        });

    }).run();
};