package com.prologin.login.Services;

import org.springframework.stereotype.Service;

import java.util.Properties;
import javax.mail.*;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeMessage;
import java.util.Properties;

@Service
public class MailSender {
    public static Boolean sendEmail(String to, String subject, String content) {
        Boolean status = false;

        // Sender's email ID and password
        final String from = "psstrainee@gmail.com";
        final String password = "byfv fmbr idmr cgxi";

        // Setup properties for Gmail SMTP
        Properties props = new Properties();
        props.put("mail.smtp.host", "smtp.gmail.com");
        props.put("mail.smtp.port", "587"); // Gmail SMTP port
        props.put("mail.smtp.auth", "true");
        props.put("mail.smtp.starttls.enable", "true"); // Enable TLS

        // Get the Session object with authentication
        Session session = Session.getInstance(props, new javax.mail.Authenticator() {
            protected PasswordAuthentication getPasswordAuthentication() {
                return new PasswordAuthentication(from, password);
            }
        });

        try {
            // Create a default MimeMessage object
            Message message = new MimeMessage(session);

            // Set From: header field
            message.setFrom(new InternetAddress(from));

            // Set To: header field
            message.setRecipients(Message.RecipientType.TO, InternetAddress.parse(to));

            // Set Subject: header field
            message.setSubject(subject);

            // Set the actual message
            message.setText(content);

            // Send message
            Transport.send(message);
            status = true;

            System.out.println("Email sent successfully @ " + to);
        } catch (MessagingException e) {
            throw new RuntimeException(e);
        } finally {
            return status;
        }
    }
}
