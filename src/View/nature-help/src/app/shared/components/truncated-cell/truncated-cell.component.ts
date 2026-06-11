import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  ElementRef,
  Input,
  OnChanges,
  OnDestroy,
  ViewChild,
} from "@angular/core";

@Component({
  selector: "nat-truncated-cell",
  templateUrl: "./truncated-cell.component.html",
  styleUrls: ["./truncated-cell.component.css"],
  standalone: false,
})
export class TruncatedCellComponent implements AfterViewInit, OnChanges, OnDestroy {
  @Input() value: string | number | null | undefined;
  @Input() fallback = "";

  @ViewChild("content") contentRef!: ElementRef<HTMLElement>;

  isOverflowing = false;

  private resizeObserver?: ResizeObserver;
  private overflowUpdateScheduled = false;

  constructor(private cdr: ChangeDetectorRef) {}

  get text(): string {
    const value = this.value;
    if (value === null || value === undefined || value === "") {
      return this.fallback;
    }

    return String(value);
  }

  ngAfterViewInit(): void {
    this.resizeObserver = new ResizeObserver(() => this.scheduleOverflowUpdate());
    this.resizeObserver.observe(this.contentRef.nativeElement);
    this.scheduleOverflowUpdate();
  }

  ngOnChanges(): void {
    this.scheduleOverflowUpdate();
  }

  ngOnDestroy(): void {
    this.resizeObserver?.disconnect();
  }

  private scheduleOverflowUpdate(): void {
    if (this.overflowUpdateScheduled) {
      return;
    }

    this.overflowUpdateScheduled = true;
    setTimeout(() => {
      this.overflowUpdateScheduled = false;
      this.updateOverflow();
    });
  }

  private updateOverflow(): void {
    const element = this.contentRef?.nativeElement;
    if (!element) {
      return;
    }

    const overflowing = element.scrollWidth > element.clientWidth;
    if (this.isOverflowing === overflowing) {
      return;
    }

    this.isOverflowing = overflowing;
    this.cdr.markForCheck();
  }
}
