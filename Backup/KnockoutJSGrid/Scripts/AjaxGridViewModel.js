var Grid = function (url, sort, filterData) {
    var self = this;
    this.rows = ko.observableArray();
    this.filterData = filterData || function () { return {}; };

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
                    self.sorting.Distinct('asc');
                    self.sorting.Field(field);
                }
            });
        }
    };

    this.update = function (data) {
        this.updateData = data;

        var c = this._counter();
        this._counter(c + 1);
    };

    this._counter = ko.observable(0);

    ko.computed(function () {
        var data = this.filterData();
        data.pageNumber = this.paging.PageNumber();
        data.sort = ko.toJS(this.sorting);

        this._counter();

        data = $.extend(data, this.updateData || {});
        
        $.ajax({
            url: url,
            type: 'POST',
            data: ko.toJSON(data),
            context: this,
            contentType: 'application/json',
            success: function (response) {
                this.rows(response.Data);
                this.paging.PageNumber(response.Paging.PageNumber);
                this.paging.TotalPagesCount(response.Paging.TotalPagesCount);
            }
        });

    }, self).extend({ throttle: 1 });


    ko.computed(function () {
        this.filterData();
        this.paging.PageNumber(1);
    }, this); 

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