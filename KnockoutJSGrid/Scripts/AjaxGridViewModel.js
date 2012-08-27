var Grid = function (url, sort, filterData) {
    var self = this;
    this.rows = ko.observableArray();
    this.filterData = filterData || function () { return { }; };
    // this.filterParams = ko.mapping.fromJS(filterParams);

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




    ko.computed(function () {
        var data = this.filterData();
        //ko.toJS(this.filterParams);
        data.pageNumber = this.paging.PageNumber();
        data.sort = ko.toJS(this.sorting);

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
        // location.hash = this.paging.PageNumber();
    }, self).extend({ throttle: 1 }); //fix 2 request http://knockoutjs.com/documentation/extenders.html


    ko.computed(function () {
        //     ko.toJS(this.filterParams);
        this.filterData();
        this.paging.PageNumber(1);
    }, this); //todo: fix me

    //    Sammy(function () {
    //        this.get('#:page', function () {
    //            debugger;
    //            var page = parseInt(this.params.page, 10);
    //            self.paging.PageNumber(page);

    //        });

    //    }).run();
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