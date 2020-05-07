import { Pipe, PipeTransform, ChangeDetectorRef, NgZone } from '@angular/core';

@Pipe({
  name: 'dateAgo',
  pure: false
})
export class DateAgoPipe implements PipeTransform {
  private timer: number;


  constructor(private changeDetectorRef: ChangeDetectorRef, private ngZone: NgZone) { }

  private dateTemplate = {
    years: ['год', 'года', 'лет'],
    months: ['месяц', 'месяца', 'месяцев'],
    weeks: ['неделя', 'недели', 'недель'],
    days: ['день', 'дня', 'дней'],
    hours: ['час', 'часа', 'часов'],
    minutes: ['минута', 'минуты', 'минут'],
    seconds: ['секунда', 'секунды','секунд']
  }

  private readonly intervals = {
    years: 365 * 24 * 60 * 60,
    months: (52 * 7 * 24 * 60 * 60) / 12,
    weeks: 7 * 24 * 60 * 60,
    days: 24 * 60 * 60,
    hours: 60 * 60,
    minutes: 60,
    seconds: 1
  }

  transform(value: any): any {
    this.removeTimer();

    if (value) {
      const seconds = Math.floor((+new Date() - +new Date(value)) / 1000);
      this.setUpdateTimer(seconds);
      if (seconds < 10) // less than 30 seconds ago will show as 'Just now'
        return 'Только что';
      let counter: number;
      for (const i in this.intervals) {
        counter = Math.floor(seconds / this.intervals[i]);
        if (counter > 0)
          return this.getFormattedDateAgo(counter, i);
      }
    }
    return value;
  }


  private setUpdateTimer(totalSeconds: number) {
    const timeToUpdate = this.getSecondsUntilUpdate(totalSeconds) * 1000;
    if (timeToUpdate)
    {
      this.timer = this.ngZone.runOutsideAngular(() => {
        if (typeof window !== 'undefined') {
          return window.setTimeout(() => {
            this.ngZone.run(() => this.changeDetectorRef.markForCheck());
          }, timeToUpdate);
        }
        return null;
      });
    }
  }

  private getFormattedDateAgo(timeUnit: number, dateTemplateKey: string): string {
    let value = timeUnit % 100;
    let formatedString: string;

    if (value > 10 && value < 15)
      formatedString = this.dateTemplate[dateTemplateKey][2];
    else {
      value %= 10;
      if (value === 1)
        formatedString = this.dateTemplate[dateTemplateKey][0];
      else if (value > 1 && value < 5)
        formatedString = this.dateTemplate[dateTemplateKey][1];
      else
        formatedString = this.dateTemplate[dateTemplateKey][2];
    }
    return `${timeUnit} ${formatedString} назад`;
  }

  private removeTimer() {
    if (this.timer) {
      window.clearTimeout(this.timer);
      this.timer = null;
    }
  }
  private getSecondsUntilUpdate(seconds: number): number {
    if (seconds < this.intervals.minutes) { // less than 1 min, update every 2 secs
      return 5;
    } else if (seconds < this.intervals.hours) { // < hour => update 1 minute
      return this.intervals.minutes;
    } else if (seconds < this.intervals.days) { // < day => update 1 hour
      return this.intervals.hours;
    } else { 
      return null;
    }
  }
}
