var AjaxGridViewModel = function (url, filterParams, sort) {
    var self = this;
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

    this.sorting = ko.mapping.fromJS(sort);
    this.checkSort = function (fieldSort) {
        if (this.sorting.Field() == fieldSort) {
            return this.sorting.Distinct() == 'asc' ? 'sorting_desc' : 'sorting_asc';
        }
        return '';
    };

    ko.bindingHandlers.sort = {
        init: function (element, valueAccessor) {
            $(element).click(function () {
                var field = valueAccessor();
                var dist = self.sorting.Distinct();
                if (field == self.sorting.Field()) {
                    self.sorting.Distinct(dist == 'asc' ? 'desc' : 'asc');
                }
                else {
                    //fix 2 request 
                    ko.utils.unwrapObservable(self.sorting.Distinct('asc'));
                    self.sorting.Field(field);
                }
            });
        }
    };



    ko.dependentObservable(function () {
        var data = ko.utils.unwrapObservable(this.filterParams);
        data.pageNumber = this.paging.PageNumber();
        data.sort = ko.toJS(this.sorting);
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

    ko.dependentObservable(function () {
        ko.toJS(this.filterParams);
        this.paging.PageNumber(1);
    }, this);


    Sammy(function () {
        this.get('#:page', function () {
            var page = parseInt(this.params.page, 10);
            self.paging.PageNumber(page);
        });

    }).run();
};




ko.bindingHandlers['class'] = {
    'update': function (element, valueAccessor) {
        if (element['__ko__previousClassValue__']) {
            $(element).removeClass(element['__ko__previousClassValue__']);
        }
        var value = ko.utils.unwrapObservable(valueAccessor());
        $(element).addClass(value);
        element['__ko__previousClassValue__'] = value;
    }
};