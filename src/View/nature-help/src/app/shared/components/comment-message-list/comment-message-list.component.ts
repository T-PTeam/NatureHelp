import { IDeficiency } from "@/models/IDeficiency";
import { ICommentMessage } from "@/models/ICommentMessage";
import { Component, Input } from "@angular/core";
import { CommentAPIService } from "@/shared/services/comment-message-api.service";
import { EDeficiencyType } from "@/models/enums";
import { MatDialog } from "@angular/material/dialog";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";

@Component({
  selector: "nat-comment-message-list",
  templateUrl: "./comment-message-list.component.html",
  styleUrls: ["./comment-message-list.component.css"],
  standalone: false,
})
export class CommentMessageListComponent {
  @Input() deficiency!: IDeficiency;
  @Input() comments: ICommentMessage[] = [];

  commentForm: FormGroup;
  submitting = false;

  constructor(
    private commentService: CommentAPIService,
    private fb: FormBuilder,
  ) {
    this.commentForm = this.fb.group({
      message: ["", [Validators.required, Validators.maxLength(1000)]],
    });
  }

  ngOnInit() {
    if (this.deficiency) {
      this.loadComments(this.deficiency.id, this.deficiency.type);
    }
  }

  loadComments(deficiencyId: string, deficiencyType: EDeficiencyType) {
    if (!deficiencyId) {
      console.warn("No deficiency ID provided for loading comments");
      return;
    }
    this.commentService.getComments(deficiencyId, deficiencyType).subscribe({
      next: (comments) => {
        this.comments = comments || [];
      },
      error: (error) => {
        console.error("Error loading comments:", error);
        this.comments = [];
      },
    });
  }

  submitComment() {
    if (this.commentForm.invalid || this.submitting) return;
    this.submitting = true;
    const message = this.commentForm.value.message;
    this.commentService
      .addComment({
        message,
        deficiencyId: this.deficiency.id,
        deficiencyType: this.deficiency.type,
        creatorFullName: sessionStorage.getItem("fullName") || "Unknown",
      })
      .subscribe({
        next: () => {
          this.commentForm.reset();
          this.loadComments(this.deficiency.id, this.deficiency.type);
          this.submitting = false;
        },
        error: (err) => {
          this.submitting = false;
        },
      });
  }

  isImageFile(mimeType: string): boolean {
    return mimeType.startsWith("image/");
  }

  isPdfFile(mimeType: string): boolean {
    return mimeType === "application/pdf";
  }
}
