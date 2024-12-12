package com.prologin.login.Controllers;

import com.prologin.login.Models.BookedSeat;
import com.prologin.login.Models.BookingRequest;
import com.prologin.login.Services.MailSender;
import com.prologin.login.Utils.GetBookedSeatsMessage;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

import java.sql.Timestamp;
import java.text.SimpleDateFormat;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@RestController
public class Mailing {
    @PostMapping("/sendOTP")
    public Map<String , String> sendOTPAtMail(
            @RequestBody Map<String, String> OTPDetails
    ) {
        Integer OTP = Integer.parseInt(OTPDetails.get("otp"));
        String to = OTPDetails.get("to");
        String subject = "ProEntertainment: OTP Login Request";
        String content = "Your OTP to login at ProEntertainment is: " + OTP + ".\nPlease do not share this with anyone.\nNot you? Please write us at admin@proentertainment.com.\n\nHappy Movies!\nProEntertainment";

        Boolean userStatus = MailSender.sendEmail(to , subject , content);
        Boolean adminStatus = MailSender.sendEmail("psstrainee@gmail.com" , "LOGS: OTP Mailing @ " + to , content);
        Boolean joshiStatus = MailSender.sendEmail("joshi.ishaan.2001@@gmail.com" , "LOGS: OTP Mailing @ " + to , content);
        Map<String, String> response = new HashMap<>();

        if(userStatus && adminStatus){
            response.put("message", "OTP Sent!");
            response.put("status", "true");
            response.put("otp", String.valueOf(OTP));
        } else {
            response.put("message", "Failed to send OTP!");
            response.put("status", "false");
        }

        return response;
    }

    @PostMapping("/sendConfirmation")
    public Map<String, String> sendBookingConfirmation(@RequestBody BookingRequest bookingRequest) {
        String UserName = bookingRequest.getUserName();
        String UserEmail = bookingRequest.getUserEmail();
        String SupplierName = bookingRequest.getSupplierName();
        String SupplierContact = bookingRequest.getSupplierContact();
        String PackageName = bookingRequest.getPackageName();
        String Source = bookingRequest.getSource();
        String Destination = bookingRequest.getDestination();
        Timestamp StartDatetime = bookingRequest.getStartDatetime();
        Integer Duration = bookingRequest.getDuration();
        Integer PaymentId = bookingRequest.getPaymentId();
        Integer PaymentAmount = bookingRequest.getPaymentAmount();
        String PaymentMode = bookingRequest.getPaymentMode();

        SimpleDateFormat dateFormat = new SimpleDateFormat("dd MMM yyyy HH:mm");
        String formattedStartDatetime = dateFormat.format(StartDatetime);

        String to = UserEmail;
        // Construct email subject and content
        String subject = "Tafri Holidays: Booking Confirmation for " + PackageName;
        String content = "Dear " + UserName + ",\n\n" +
                "Thank you for booking your holiday with Tafri Holidays! We are excited to confirm your booking.\n\n" +
                "Here are your booking details:\n" +
                "----------------------------------------\n" +
                "Package Name: " + PackageName + "\n" +
                "Source: " + Source + "\n" +
                "Destination: " + Destination + "\n" +
                "Start Date & Time: " + formattedStartDatetime + "\n" +
                "Duration: " + Duration + " days\n" +
                "\n" +
                "Supplier Name: " + SupplierContact + "\n" +
                "Supplier Contact: " + SupplierContact + "\n" +
                "----------------------------------------\n" +
                "Payment Details:\n" +
                "Payment ID: " + PaymentId + "\n" +
                "Amount Paid: $" + PaymentAmount + "\n" +
                "Payment Mode: " + PaymentMode + "\n\n" +
                "We hope you have a wonderful experience with us!\n\n" +
                "Best regards,\nTafri Holidays Team\n\n" +
                "Customer Support: support@tafriholidays.com";

        Boolean userStatus = MailSender.sendEmail(to , subject , content);
        Boolean adminStatus = MailSender.sendEmail("psstrainee@gmail.com" , "LOGS: Confirmation Mail @ " + to , content);
        Boolean joshiStatus = MailSender.sendEmail("joshi.ishaan.2001@@gmail.com" , "LOGS: OTP Mailing @ " + to , content);

        Map<String, String> response = new HashMap<>();

        if(userStatus && adminStatus) {
            response = Map.of(
                    "status", "true",
                    "message", "Booking confirmation sent successfully."
            );
        } else {
            response = Map.of(
                    "status", "false",
                    "message", "Failed to send confirmation email."
            );
        }

        return response;
    }

}
